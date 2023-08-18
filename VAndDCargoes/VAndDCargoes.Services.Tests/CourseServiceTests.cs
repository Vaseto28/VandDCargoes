using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Services.Models;
using static VAndDCargoes.Services.Tests.DbSeeder;

namespace VAndDCargoes.Services.Tests;

public class CourseServiceTests
{
    private DbContextOptions<VAndDCargoesDbContext> options;
    private VAndDCargoesDbContext dbCtx;

    private ICourseService courseService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.options = new DbContextOptionsBuilder<VAndDCargoesDbContext>()
                .UseInMemoryDatabase("VAndDCargoesInMemory" + Guid.NewGuid().ToString())
                .Options;

        this.dbCtx = new VAndDCargoesDbContext(this.options);

        this.dbCtx.Database.EnsureCreated();
        SeedDb(this.dbCtx);

        this.courseService = new CourseService(this.dbCtx);
    }

    [Test]
    public async Task FinishCourseAsyncShouldReturnNullIfDriverDoesNotExist()
    {
        DeleteCourseModel? expectedResult = await this.courseService.FinishCourseAsync("21e1c88d-eb11-42e6-8e01-4610ab2b958d", "64e2f7b0-9f9e-4e7c-aa61-8525bfb5ddf2");

        Assert.That(expectedResult == null!);
    }

    [Test]
    public async Task FinishCourseAsyncShouldReturnNullIfCourseDoesNotExist()
    {
        DeleteCourseModel? expectedResult = await this.courseService.FinishCourseAsync("474cd8b7-20d0-4c06-98f3-20bc5f32b992", "21e1c88d-eb11-42e6-8e01-4610ab2b958");

        Assert.That(expectedResult == null!);
    }

    [Test]
    public async Task FinishCourseAsyncShouldReturnNewDeleteCourseModel()
    {
        await this.dbCtx.Trailers.AddAsync(DbSeeder.Trailer);
        await this.dbCtx.Cargoes.AddAsync(DbSeeder.Cargo); 
        await this.dbCtx.Courses.AddAsync(DbSeeder.Course);
        await this.dbCtx.DriversCargoes.AddAsync(DbSeeder.DriversCargoes);
        await this.dbCtx.SaveChangesAsync();

        List<Course> courses = await this.dbCtx.Courses.ToListAsync();

        string courseId = courses[0].Id.ToString();
        string userId = courses[0].Driver.UserId.ToString();

        DeleteCourseModel? expectedResult = await this.courseService.FinishCourseAsync(userId, courseId);

        Assert.That(expectedResult.CargoId.Equals(courses[0].CargoId.ToString()));
        Assert.That(expectedResult.Reward.Equals(650));
    }

    [Test]
    public async Task IsTheCargoAvailableShouldReturnFalseIfDriverDoesNotExist()
    {
        bool actualResult = await this.courseService.IsCargoAvailableByIdAsync("21e1c88d-eb11-42e6-8e01-4610ab2b958d", DbSeeder.Cargo.Id.ToString());

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task IsTheCargoAvailableShouldReturnFalseIfCargoExists()
    {
        DbSeeder.Driver.Courses.Add(DbSeeder.Course);

        bool actualResult = await this.courseService.IsCargoAvailableByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Cargo.Id.ToString());

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task IsTheCargoAvailableShouldReturnTrueIfCargoDoesNotExists()
    {
        DbSeeder.Driver.Courses.Add(DbSeeder.Course);

        bool actualResult = await this.courseService.IsCargoAvailableByIdAsync(DbSeeder.Driver.UserId.ToString(), "21e1c88d-eb11-42e6-8e01-4610ab2b958d");

        Assert.IsTrue(actualResult);
    }

    [Test]
    public async Task IsTrailerAvailableByIdAsyncShouldReturnFalseIfDriverDoesNotExist()
    {
        bool actualResult = await this.courseService.IsTrailerAvailableByIdAsync("21e1c88d-eb11-42e6-8e01-4610ab2b958d", DbSeeder.Trailer.Id.ToString());

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task IsTrailerAvailableByIdAsyncShouldReturnFalseIfTrailerExists()
    {
        bool actualResult = await this.courseService.IsTrailerAvailableByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Trailer.Id.ToString());

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task IsTrailerAvailableByIdAsyncShouldReturnTrueIfTrailerDoesNotExists()
    {
        bool actualResult = await this.courseService.IsTrailerAvailableByIdAsync(DbSeeder.Driver.UserId.ToString(), "21e1c88d-eb11-42e6-8e01-4610ab2b958d");

        Assert.IsTrue(actualResult);
    }

    [Test]
    public async Task IsTruckAvailableByIdAsyncShouldReturnFalseIfDriverDoesNotExist()
    {
        bool actualResult = await this.courseService.IsTruckAvailableByIdAsync("21e1c88d-eb11-42e6-8e01-4610ab2b958d", DbSeeder.Truck.Id.ToString());

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task IsTruckAvailableByIdAsyncShouldReturnFalseIfTruckExists()
    {
        bool actualResult = await this.courseService.IsTruckAvailableByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Truck.Id.ToString());

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task IsTruckAvailableByIdAsyncShouldReturnTrueIfTruckDoesNotExist()
    {
        bool actualResult = await this.courseService.IsTruckAvailableByIdAsync(DbSeeder.Driver.UserId.ToString(), "21e1c88d-eb11-42e6-8e01-4610ab2b958d");

        Assert.IsTrue(actualResult);
    }

    [Test]
    public async Task TakeTheCourseAsyncShouldReturnFalseIfTruckDoesNotExist()
    {
        bool actualResult = await this.courseService.TakeTheCourseAsync(DbSeeder.DriverUser.Id.ToString(), DbSeeder.InvalidStartCourseViewModel);

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task TakeTheCourseAsyncShouldReturnFalseIfTruckConditionIsTooBad()
    {
        bool actualResult = await this.courseService.TakeTheCourseAsync(DbSeeder.DriverUser.Id.ToString(), DbSeeder.LowConditionStartCourseViewModel);

        Assert.IsFalse(actualResult);
    }

    [Test]
    public async Task TakeTheCourseAsyncShouldReturnFalseIfDriverDoesNotExist()
    {
        bool actualResult = await this.courseService.TakeTheCourseAsync("21e1c88d-eb11-42e6-8e01-4610ab2b958d", DbSeeder.StartCourseViewModel);

        Assert.IsFalse(actualResult);
    }
}

