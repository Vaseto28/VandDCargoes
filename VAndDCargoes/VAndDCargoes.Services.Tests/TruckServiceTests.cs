using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Truck;
using static VAndDCargoes.Services.Tests.DbSeeder;

namespace VAndDCargoes.Services.Tests;

public class TruckServiceTests
{
    private DbContextOptions<VAndDCargoesDbContext> options;
    private VAndDCargoesDbContext dbCtx;

    private ITruckService truckService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.options = new DbContextOptionsBuilder<VAndDCargoesDbContext>()
                .UseInMemoryDatabase("VAndDCargoesInMemory" + Guid.NewGuid().ToString())
                .Options;

        this.dbCtx = new VAndDCargoesDbContext(this.options);

        this.dbCtx.Database.EnsureCreated();
        SeedDb(this.dbCtx);

        this.truckService = new TruckService(this.dbCtx);
    }

    [Test]
    public async Task AddTruckAsyncShouldCreateTruckAndAddItToTheDatabase()
    {
        await this.truckService.AddTruckAsync(DbSeeder.TruckAddViewModel, "0c87b02f-3c9b-456a-bf1d-83ec84739da0");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 2;
        int actualResult = this.dbCtx.Trucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeleteByIdAsyncShouldDeleteTruckFromTheDatabaseIfItExists()
    {
        Truck existingTruck = await this.dbCtx.Trucks.FirstAsync(x => x.Make == "Daf");

        string existingTruckId = existingTruck.Id.ToString();

        await this.truckService.DeletebyIdAsync(existingTruckId);
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 1;
        int actualResult = this.dbCtx.Trucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeleteByIdAsyncShouldDoNothingIfItDoesNotExist()
    {
        await this.truckService.AddTruckAsync(DbSeeder.TruckAddViewModel, "0c87b02f-3c9b-456a-bf1d-83ec84739da0");

        await this.truckService.DeletebyIdAsync("c65a6287-5d71-4f09-9512-38edf4cb9f58");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 2;
        int actualResult = this.dbCtx.Trucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DriveTruckByIdAsyncShouldMoveTheTruckToTheDriversDrivenTrucksIfBothExists()
    {
        await this.truckService.DriveTruckByIdAsync(DbSeeder.Driver.User.Id.ToString(), DbSeeder.Truck.Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 1;
        int actualResult = this.dbCtx.DriversTrucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DriveTruckByIdAsyncShouldDoNothingIfTruckDoesNotExists()
    {
        await this.truckService.DriveTruckByIdAsync(DbSeeder.Driver.User.Id.ToString(), "42408c4f-78b3-447e-89d7-53f34aacba2f");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 0;
        int actualResult = this.dbCtx.DriversTrucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DriveTruckByIdAsyncShouldDoNothingIfUserDoesNotExistsOrIsNotADriver()
    {
        await this.truckService.DriveTruckByIdAsync("bf822f2b-8f75-4a3c-bc82-900a607d6647", DbSeeder.Truck.Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 0;
        int actualResult = this.dbCtx.DriversTrucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task EditTruckAsyncShouldEditTruckIfExists()
    {
        Truck existingTruck = await this.dbCtx.Trucks.FirstAsync(x => x.Make == "Daf");

        string existingTruckId = existingTruck.Id.ToString();

        await this.truckService.EditTruckAsync(DbSeeder.TruckEditViewModel, existingTruckId);
        await this.dbCtx.SaveChangesAsync();

        int expectedCondition = 3;
        int actualCondition = (int)existingTruck.Condition;

        string expectedCreatedOn = "11/23/2009";
        string actualCreatedOn = existingTruck.CreatedOn.ToString("MM/dd/yyyy");

        Assert.That(expectedCondition.Equals(actualCondition));
        Assert.That(expectedCreatedOn.Equals(actualCreatedOn));
    }

    [Test]
    public async Task EditTruckAsyncShouldDoNothingIfTruckDoesNotExist()
    {
        await this.truckService.EditTruckAsync(DbSeeder.TruckEditViewModel, "141f570d-d79c-4da7-acea-4f1a22fad8d3");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 2;
        int actualResult = this.dbCtx.Trucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task GetTruckByIdForEditAsyncShouldGetATruckToEditIfItExists()
    {
        TruckEditViewModel expectedResult = new TruckEditViewModel()
        {
            Make = DbSeeder.Truck.Make,
            Model = DbSeeder.Truck.Model,
            RegistrationNumber = DbSeeder.Truck.RegistrationNumber,
            Condition = (int)DbSeeder.Truck.Condition,
            TravelledDistance = DbSeeder.Truck.TraveledDistance,
            FuelCapacity = DbSeeder.Truck.FuelCapacity,
            CreatedOn = DbSeeder.Truck.CreatedOn.ToString("MM/dd/yyyy"),
            ImageUrl = DbSeeder.Truck.ImageUrl
        };
        TruckEditViewModel actualResult = await this.truckService.GetTruckByIdForEditAsync(DbSeeder.Truck.Id.ToString());

        Assert.That(expectedResult.Make.Equals(actualResult.Make));
        Assert.That(expectedResult.Model.Equals(actualResult.Model));
        Assert.That(expectedResult.RegistrationNumber.Equals(actualResult.RegistrationNumber));
        Assert.That(expectedResult.Condition.Equals(actualResult.Condition));
        Assert.That(expectedResult.TravelledDistance.Equals(actualResult.TravelledDistance));
        Assert.That(expectedResult.FuelCapacity.Equals(actualResult.FuelCapacity));
        Assert.That(expectedResult.CreatedOn.Equals(actualResult.CreatedOn));
        Assert.That(expectedResult.ImageUrl.Equals(actualResult.ImageUrl));
    }

    [Test]
    public async Task GetTruckByIdForEditAsyncShouldReturnNullIfItDoesNotExists()
    {
        TruckEditViewModel? expectedResult = null;
        TruckEditViewModel? actualResult = await this.truckService.GetTruckByIdForEditAsync("97a1e1c1-c7c9-4386-b871-07541c3e8334");

        Assert.That(expectedResult == actualResult);
    }

    //[Test]
    //public async Task GetTruckDetailsByIdAsyncShouldReturnTruckDetailsViewModelIfItExists()
    //{
    //    TruckDetailsViewModel expectedResult = new TruckDetailsViewModel()
    //    {
    //        Id = DbSeeder.Truck.Id.ToString(),
    //        Make = DbSeeder.Truck.Make,
    //        Model = DbSeeder.Truck.Model,
    //        RegistrationNumber = DbSeeder.Truck.RegistrationNumber,
    //        TravelledDistance = DbSeeder.Truck.TraveledDistance,
    //        FuelCapacity = DbSeeder.Truck.FuelCapacity,
    //        Condition = DbSeeder.Truck.Condition.ToString(),
    //        CreatedOn = DbSeeder.Truck.CreatedOn.ToString("dd/MM/yyyy"),
    //        ImageUrl = DbSeeder.Truck.ImageUrl
    //    };

    //    TruckDetailsViewModel? actualResult = await this.truckService.GetTruckDetailsByIdAsync(DbSeeder.Truck.Id.ToString());

    //    Assert.That(expectedResult.Id.Equals(actualResult!.Id));
    //    Assert.That(expectedResult.Make.Equals(actualResult.Make));
    //    Assert.That(expectedResult.Model.Equals(actualResult.Model));
    //    Assert.That(expectedResult.RegistrationNumber.Equals(actualResult.RegistrationNumber));
    //    Assert.That(expectedResult.Condition.Equals(actualResult.Condition));
    //    Assert.That(expectedResult.TravelledDistance.Equals(actualResult.TravelledDistance));
    //    Assert.That(expectedResult.FuelCapacity.Equals(actualResult.FuelCapacity));
    //    Assert.That(expectedResult.CreatedOn.Equals(actualResult.CreatedOn));
    //    Assert.That(expectedResult.ImageUrl.Equals(actualResult.ImageUrl));
    //}

    [Test]
    public void IsConditionValidShouldReturnFalseIfConditionNumberIsValid()
    {
        Truck truckToAdd = new Truck()
        {
            Make = "Mercedes",
            Model = "NewActros",
            RegistrationNumber = "Sw wdoma 21",
            Condition = (TruckCondition)3,
            FuelCapacity = 800,
            TraveledDistance = 1000,
            ImageUrl = "https://www.daf.co.uk/-/media/images/daf-trucks/trucks/euro-6/daf-xf/2017035-daf-xf.jpg?mw=1200&rev=e21fa83ff62d405b9db95e6914c636e1&hash=5102BA118103505BCA802B2D812C22BD",
            CreatedOn = DateTime.Now,
            CreatorId = new Guid("2f546a89-be24-4111-bff8-769bbfe1132a")
        };

        bool actualResult = this.truckService.IsConditionValid((int)truckToAdd.Condition);

        Assert.IsFalse(actualResult);
    }

    [Test]
    public void IsConditionValidShouldReturnTrueIfConditionNumberIsInvalid()
    {
        Truck truckToAdd = new Truck()
        {
            Make = "Mercedes",
            Model = "NewActros",
            RegistrationNumber = "Sw wdoma 21",
            Condition = (TruckCondition)1221,
            FuelCapacity = 800,
            TraveledDistance = 1000,
            ImageUrl = "https://www.daf.co.uk/-/media/images/daf-trucks/trucks/euro-6/daf-xf/2017035-daf-xf.jpg?mw=1200&rev=e21fa83ff62d405b9db95e6914c636e1&hash=5102BA118103505BCA802B2D812C22BD",
            CreatedOn = DateTime.Now,
            CreatorId = new Guid("2f546a89-be24-4111-bff8-769bbfe1132a")
        };

        bool actualResult = this.truckService.IsConditionValid((int)truckToAdd.Condition);

        Assert.IsTrue(actualResult);
    }

    [Test]
    public async Task ReleaseTruckByIdAsyncShouldRemoveTruckFromDriversDrivenTrucksIfBothExists()
    {
        await this.truckService.ReleaseTruckByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Driver.DriversTrucks.ToList()[0].TruckId.ToString());

        int expectedResult = 0;
        int actualResult = this.dbCtx.DriversTrucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task ReleaseTruckByIdAsyncShouldDoNothingIfDriverDoesNotExist()
    {
        await this.truckService.ReleaseTruckByIdAsync("b67c7dce-7d4f-4ab5-8109-a961faac44bb", DbSeeder.Driver.DriversTrucks.ToList()[0].TruckId.ToString());

        int expectedResult = 1;
        int actualResult = this.dbCtx.DriversTrucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task ReleaseTruckByIdAsyncShouldDoNothingIfTruckDoesNotExist()
    {
        await this.truckService.ReleaseTruckByIdAsync(DbSeeder.Driver.UserId.ToString(), "b67c7dce-7d4f-4ab5-8109-a961faac44bb");

        int expectedResult = 1;
        int actualResult = this.dbCtx.DriversTrucks.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task IsUserAlreadyDrivingTruckByIdAsyncShouldReturnFalseIfTheDriverIsAlreadyDrivingThisTruck()
    { 
        Assert.IsFalse(await this.truckService.IsUserAlreadyDrivingTruckByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Driver.DriversTrucks.ToList()[0].TruckId.ToString()));
    }

    [Test]
    public async Task IsUserAlreadyDrivingTruckByIdAsyncShouldReturnTrueIfTheDriverIsNotDrivingThisTruckYet()
    {
        Assert.IsFalse(await this.truckService.IsUserAlreadyDrivingTruckByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Truck.Id.ToString()));
    }
}

