using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Course;

namespace VAndDCargoes.Services;

public class CourseService : ICourseService
{
    private readonly VAndDCargoesDbContext dbContext;

    public CourseService(VAndDCargoesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<decimal> FinishCourseAsync(string userId, string courseId)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            Course? course = await this.dbContext.Courses
                .FindAsync(Guid.Parse(courseId));

            if (course != null)
            {
                driver.Ballance += course.Reward;
                course.Truck.TraveledDistance += course.Distance;

                this.dbContext.DriversCargoes.Remove(driver.DriversCargoes.FirstOrDefault(x => x.CargoId.Equals(course.CargoId)));

                driver.Courses.Remove(course);

                this.dbContext.Courses.Remove(course);
                await this.dbContext.SaveChangesAsync();

                return course.Reward;
            }
        }

        return 0m;
    }

    public async Task<IEnumerable<AllCoursesViewModel>> GetAllCoursesByUserIdAsync(string userId)
    {
        return await this.dbContext.Courses
            .Where(x => x.Driver.UserId.ToString().Equals(userId))
            .Select(x => new AllCoursesViewModel()
            {
                Id = x.Id.ToString(),
                DepartureCity = x.DepartureCity,
                ArrivalCity = x.ArrivalCity,
                Distance = x.Distance,
                Reward = x.Reward,
                Truck = x.Truck.Make + " " + x.Truck.Model,
                Cargo = x.Cargo.Name
            })
            .ToArrayAsync();
    }

    public async Task<bool> IsTheCargoAvailable(string userId, string cargoId)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            Cargo? cargo = driver.Courses
                .FirstOrDefault(x => x.CargoId.ToString().Equals(cargoId))?.Cargo;

            if (cargo == null)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> IsTheTrailerAvailableAsync(string userId, string trailerId)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            Trailer? trailer = driver.Courses
                .FirstOrDefault(x => x.TrailerId.ToString().Equals(trailerId))?.Trailer;

            if (trailer == null)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> IsTheTruckAvailableAsync(string userId, string truckId)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            Truck? truck = driver.Courses
                .FirstOrDefault(x => x.TruckId.ToString().Equals(truckId))?.Truck;

            if (truck == null)
            {
                return true;
            }
        }

        return false;
    }

    public async Task TakeTheCourseAsync(string userId, StartCourseViewModel model)
    {
        Driver? driver = await this.dbContext.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            Course course = new Course()
            {
                DepartureCity = model.DepartureCity,
                ArrivalCity = model.ArrivalCity,
                Distance = model.Distance,
                Reward = 500 + model.Distance * 0.6m,
                DriverId = driver.Id,
                TruckId = Guid.Parse(model.TruckId),
                TrailerId = Guid.Parse(model.TrailerId),
                CargoId = Guid.Parse(model.CargoId)
            };

            await this.dbContext.Courses.AddAsync(course);
            await this.dbContext.SaveChangesAsync();
        }
    }
}

