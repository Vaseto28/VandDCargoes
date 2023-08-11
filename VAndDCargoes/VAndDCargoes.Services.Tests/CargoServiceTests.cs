using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Cargo;
using static VAndDCargoes.Services.Tests.DbSeeder;

namespace VAndDCargoes.Services.Tests;

public class CargoServiceTests
{
    private DbContextOptions<VAndDCargoesDbContext> options;
    private VAndDCargoesDbContext dbCtx;

    private ICargoService cargoService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.options = new DbContextOptionsBuilder<VAndDCargoesDbContext>()
                .UseInMemoryDatabase("VAndDCargoesInMemory" + Guid.NewGuid().ToString())
                .Options;

        this.dbCtx = new VAndDCargoesDbContext(this.options);

        this.dbCtx.Database.EnsureCreated();
        SeedDb(this.dbCtx);

        this.cargoService = new CargoService(this.dbCtx);
    }

    [Test]
    public async Task AddCargoAsyncShouldCreateNewCargoAndAddItToTheDbIfDataIsValid()
    {
        await this.cargoService.AddCargoAsync(DbSeeder.CargoAddViewModel, "061f4e83-853f-4f44-aad4-8b526aad1993");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 1;
        int actualResult = this.dbCtx.Cargoes.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeleteCargoByIdAsyncShouldRemoveTheGivenCargoFromTheDbIfExists()
    {
        Cargo cargo = await this.dbCtx.Cargoes.FirstAsync(x => x.Name == "Woods");

        await this.cargoService.DeleteCargoByIdAsync(cargo.Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 0;
        int actualResult = this.dbCtx.Cargoes.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeleteCargoByIdAsyncShouldDoNothingIfCargoIdDoesNotExistInDb()
    {
        await this.cargoService.DeleteCargoByIdAsync("cfa73a5c-0a71-4ab2-8893-e295a0636769");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 1;
        int actualResult = this.dbCtx.Cargoes.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeliverCargoByIdAsyncShouldAddCargoToTheDriversCargoesIfBothExists()
    {
        await this.cargoService.AddCargoAsync(DbSeeder.CargoAddViewModel, DbSeeder.Driver.UserId.ToString());
        bool result = await this.cargoService.DeliverCargoByIdAsync(DbSeeder.Driver.UserId.ToString(), this.dbCtx.Cargoes.ToList()[0].Id.ToString());

        int expectedResult = 1;
        int actualResult = DbSeeder.Driver.DriversCargoes.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsTrue(result);
    }

    [Test]
    public async Task DeliverCargoByIdAsyncShouldDoNothingIfDriverDoesNotExist()
    {
        bool result = await this.cargoService.DeliverCargoByIdAsync("4444de6c-1016-4622-bcb9-53544e230e9a", this.dbCtx.Cargoes.ToList()[0].Id.ToString());

        int expectedResult = 1;
        int actualResult = this.dbCtx.DriversCargoes.Count();

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsFalse(result);
    }

    [Test]
    public async Task DeliverCargoByIdAsyncShouldDoNothingIfCargoDoesNotExist()
    {
        bool result = await this.cargoService.DeliverCargoByIdAsync(DbSeeder.Driver.UserId.ToString(), "4444de6c-1016-4622-bcb9-53544e230e9a");

        int expectedResult = 1;
        int actualResult = this.dbCtx.DriversCargoes.Count();

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsFalse(result);
    }

    [Test]
    public async Task EditCargoByIdAsyncShouldUpdateAnExistingCargo()
    {
        await this.cargoService.EditCargoAsync(DbSeeder.CargoEditViewModel, this.dbCtx.Cargoes.ToList()[0].Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        string expectedName = "Wood";
        string actualName = this.dbCtx.Cargoes.ToList()[0].Name;

        int expectedWeight = 20;
        int actualWeight = this.dbCtx.Cargoes.ToList()[0].Weight;

        string expectedDescription = "Very big woods, which require driver with many expiriences and very good truck and trailer";
        string actualDescription = this.dbCtx.Cargoes.ToList()[0].Description;

        Assert.That(expectedName.Equals(actualName));
        Assert.That(expectedWeight.Equals(actualWeight));
        Assert.That(expectedDescription.Equals(actualDescription));
    }

    [Test]
    public async Task EditCargoByIdAsyncShouldDoNothingIfCargoeDoesNotExist()
    {
        await this.cargoService.EditCargoAsync(DbSeeder.CargoEditViewModel, "91f5d58f-05e8-4b4f-ab43-60366a6a4dc9");
        await this.dbCtx.SaveChangesAsync();

        string expectedName = "Woods";
        string actualName = this.dbCtx.Cargoes.ToList()[0].Name;

        int expectedWeight = 22;
        int actualWeight = this.dbCtx.Cargoes.ToList()[0].Weight;

        string expectedDescription = "Very very big woods, requiring driver with many expoiriences and very good truckm and trailer";
        string actualDescription = this.dbCtx.Cargoes.ToList()[0].Description;

        Assert.That(expectedName.Equals(actualName));
        Assert.That(expectedWeight.Equals(actualWeight));
        Assert.That(expectedDescription.Equals(actualDescription));
    }

    [Test]
    public async Task FinishDeliveringOfCargoByIdAsyncShouldRemoveCargoOfTheDriversDeliveringCargoesIfBothExists()
    {
        bool result = await this.cargoService.FinishDeliveringOfCargoByIdAsync(DbSeeder.Driver.UserId.ToString(), this.dbCtx.Cargoes.ToList()[0].Id.ToString());

        int expectedResult = 0;
        int actualResult = DbSeeder.Driver.DriversCargoes.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsTrue(result);
    }

    [Test]
    public async Task FinishDeliveringOfCargoByIdAsyncShouldDoNothingIfDriverDoesNotExist()
    {
        bool result = await this.cargoService.FinishDeliveringOfCargoByIdAsync("2398c4b9-2e04-43f7-a5c5-8d8b91e01453", this.dbCtx.Cargoes.ToList()[0].Id.ToString());

        int expectedResult = 1;
        int actualResult = DbSeeder.Driver.DriversCargoes.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsFalse(result);
    }

    [Test]
    public async Task FinishDeliveringOfCargoByIdAsyncShouldDoNothingIfCargoDoesNotExist()
    {
        bool result = await this.cargoService.FinishDeliveringOfCargoByIdAsync(DbSeeder.Driver.UserId.ToString(), "c3b45c9b-32af-47f5-8aaa-ee2c89f1ebfd");

        int expectedResult = 1;
        int actualResult = DbSeeder.Driver.DriversCargoes.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsFalse(result);
    }

    [Test]
    public async Task GetCargoDetailsByIdShouldReturnCargoesDetailsIfExists()
    {
        CargoDetailsViewModel expectedResult = new CargoDetailsViewModel()
        {
            Id = this.dbCtx.Cargoes.ToList()[0].Id.ToString(),
            Name = this.dbCtx.Cargoes.ToList()[0].Name,
            Description = this.dbCtx.Cargoes.ToList()[0].Description,
            Weight = this.dbCtx.Cargoes.ToList()[0].Weight,
            Category = this.dbCtx.Cargoes.ToList()[0].Category.ToString(),
            PhysicalState = this.dbCtx.Cargoes.ToList()[0].PhysicalState.ToString(),
            CreatorName = this.dbCtx.Cargoes.ToList()[0].Creator.UserName,
            CreatorEmail = this.dbCtx.Cargoes.ToList()[0].Creator.Email.ToLower(),
            ImageUrl = this.dbCtx.Cargoes.ToList()[0].ImageUrl
        };

        CargoDetailsViewModel actualResult = await this.cargoService.GetCargoDetailsById(this.dbCtx.Cargoes.ToList()[0].Id.ToString());

        Assert.That(expectedResult.Id.Equals(actualResult.Id));
        Assert.That(expectedResult.Name.Equals(actualResult.Name));
        Assert.That(expectedResult.Description.Equals(actualResult.Description));
        Assert.That(expectedResult.Weight.Equals(actualResult.Weight));
        Assert.That(expectedResult.Category.Equals(actualResult.Category));
        Assert.That(expectedResult.PhysicalState.Equals(actualResult.PhysicalState));
        Assert.That(expectedResult.ImageUrl.Equals(actualResult.ImageUrl));
    }

    [Test]
    public async Task GetCargoDetailsByIdShouldReturnNullIfCargoDoesNotExist()
    {
        CargoDetailsViewModel actualResult = await this.cargoService.GetCargoDetailsById("2dc93bc7-97f9-4c3e-bf07-798dd2cd8411");

        Assert.That(actualResult == null);
    }

    [Test]
    public async Task GetCargoForEditByIdAsyncShouldReturnCargoToEditIfExists()
    {
        CargoEditViewModel expectedResult = new CargoEditViewModel()
        {
            Name = this.dbCtx.Cargoes.ToList()[0].Name,
            Description = this.dbCtx.Cargoes.ToList()[0].Description,
            Weight = this.dbCtx.Cargoes.ToList()[0].Weight,
            Category = (int)this.dbCtx.Cargoes.ToList()[0].Category,
            PhysicalState = (int)this.dbCtx.Cargoes.ToList()[0].PhysicalState,
            ImageUrl = this.dbCtx.Cargoes.ToList()[0].ImageUrl
        };

        CargoEditViewModel actualResult = await this.cargoService.GetCargoForEditByIdAsync(this.dbCtx.Cargoes.ToList()[0].Id.ToString());

        Assert.That(expectedResult.Name.Equals(actualResult.Name));
        Assert.That(expectedResult.Description.Equals(actualResult.Description));
        Assert.That(expectedResult.Weight.Equals(actualResult.Weight));
        Assert.That(expectedResult.Category.Equals(actualResult.Category));
        Assert.That(expectedResult.PhysicalState.Equals(actualResult.PhysicalState));
        Assert.That(expectedResult.ImageUrl.Equals(actualResult.ImageUrl));
    }

    [Test]
    public async Task GetCargoForEditByIdAsyncShouldReturnNullIfDoesNotExist()
    {
        CargoEditViewModel actualResult = await this.cargoService.GetCargoForEditByIdAsync("876a1dd6-8c86-4f14-b120-1ed70ed67c98");

        Assert.That(actualResult == null);
    }

    [Test]
    public async Task IsCargoStillDeliveringShouldReturnFalseIfCargoeExistsInDriversDeliveringCargoesIfExists()
    {
        await this.cargoService.DeliverCargoByIdAsync(DbSeeder.Driver.UserId.ToString(), this.dbCtx.Cargoes.ToList()[0].Id.ToString());
        bool result = await this.cargoService.IsCargoStillDelivering(DbSeeder.Driver.UserId.ToString(), DbSeeder.Driver.DriversCargoes.ToList()[0].CargoId.ToString());

        Assert.IsFalse(result);
    }

    [Test]
    public async Task IsCargoStillDeliveringShouldReturnTrueIfCargoeExistsInDriversDeliveringCargoesIfExists()
    {
        bool result = await this.cargoService.IsCargoStillDelivering(DbSeeder.Driver.UserId.ToString(), "628ed0dc-61ee-4ed2-b620-337f94a0da00");

        Assert.IsTrue(result);
    }

    [Test]
    public void IsCategoryValidShouldReturFalseIfCategoryNumIsValid()
    {
        bool result = this.cargoService.IsCategoryValid(2);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsCategoryValidShouldReturTrueIfCategoryNumIsNotValid()
    {
        bool result = this.cargoService.IsCategoryValid(244);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsPhysicalStateValidShouldReturnFalseIfPhysicalStateNumberIsValid()
    {
        bool result = this.cargoService.IsPhysicalStateValid(2);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsPhysicalStateValidShouldReturnTrueIfPhysicalStateNumberIsNotValid()
    {
        bool result = this.cargoService.IsPhysicalStateValid(222);

        Assert.IsTrue(result);
    }
}

