﻿using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Services.Models;
using VAndDCargoes.Web.ViewModels.Course;

namespace VAndDCargoes.Services;

public class CourseService : ICourseService
{
    private readonly VAndDCargoesDbContext dbContext;

    public CourseService(VAndDCargoesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<DeleteCourseModel> FinishCourseAsync(string userId, string courseId)
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
                course.Truck.Condition += 1;
                course.Trailer.Condition += 1;

                this.dbContext.DriversCargoes.Remove(driver.DriversCargoes.FirstOrDefault(x => x.CargoId.Equals(course.CargoId))!);

                driver.Courses.Remove(course);

                this.dbContext.Courses.Remove(course);

                await this.dbContext.SaveChangesAsync();

                return new DeleteCourseModel()
                {
                    CargoId = course.CargoId.ToString(),
                    Reward = course.Reward
                };
            }
        }

        return null!;
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

    public async Task<bool> IsCargoAvailableByIdAsync(string userId, string cargoId)
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

    public async Task<bool> IsTrailerAvailableByIdAsync(string userId, string trailerId)
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

    public async Task<bool> IsTruckAvailableByIdAsync(string userId, string truckId)
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

    public async Task<bool> TakeTheCourseAsync(string userId, StartCourseViewModel model)
    {
        Truck? truck = await this.dbContext.Trucks
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(model.TruckId));

        if (truck != null)
        {
            if (truck.Condition >= TruckCondition.NeedOfService)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        Trailer? trailer = await this.dbContext.Trailers
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(model.TrailerId));

        if (trailer != null)
        {
            if (trailer.Condition >= TrailerCondition.NeedOfService)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

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
        else
        {
            return false;
        }

        return true;
    }
}

