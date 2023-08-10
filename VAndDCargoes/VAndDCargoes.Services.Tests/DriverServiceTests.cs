using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Driver;
using static VAndDCargoes.Services.Tests.DbSeeder;

namespace VAndDCargoes.Services.Tests;

public class DriverServiceTests
{
    private DbContextOptions<VAndDCargoesDbContext> options;
    private VAndDCargoesDbContext dbCtx;

    private IDriverService driverService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.options = new DbContextOptionsBuilder<VAndDCargoesDbContext>()
                .UseInMemoryDatabase("VAndDCargoesInMemory" + Guid.NewGuid().ToString())
                .Options;

        this.dbCtx = new VAndDCargoesDbContext(this.options);

        this.dbCtx.Database.EnsureCreated();
        SeedDb(this.dbCtx);

        this.driverService = new DriverService(this.dbCtx);
    }

    [Test]
    public async Task IsTheUseAlreadyDriverByIdAsyncShouldReturnTrueIfTheUserIsDriver()
    {
        string driverUserId = DriverUser.Id.ToString();

        bool result = await this.driverService.IsTheUseAlreadyDriverByIdAsync(driverUserId);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task IsTheUseAlreadyDriverByIdAsyncShouldReturnTrueIfTheUserIsNotDriver()
    {
        string notDriveruserId = User.Id.ToString();

        bool result = await this.driverService.IsTheUseAlreadyDriverByIdAsync(notDriveruserId);

        Assert.IsFalse(result);
    }

    [Test]
    public async Task GetTheFullNameOfDriverByUserIdAsyncShouldReturnTheFulNameOfADriverWhoExistsInTheDatabase()
    {
        string driverUserId = DriverUser.Id.ToString();

        string actualResult = await this.driverService.GetTheFullNameOfDriverByUserIdAsync(driverUserId);
        string expectedResult = "Petar Magdalenov Ivanov";

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task GetTheFullNameOfDriverByUserIdAsyncShouldReturnAnEmptyStringIfTheDriverDoesNotExistsInTheDatabase()
    {
        string notDriverUserId = User.Id.ToString();

        string actualResult = await this.driverService.GetTheFullNameOfDriverByUserIdAsync(notDriverUserId);
        string expectedResult = string.Empty;

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task BecomeDriverAsyncShouldCreateNewDriverAndAddItToTheDatabase()
    {
        await this.driverService.BecomeDriverAsync(DriverToAdd, UserBecomingDriver.Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 2;
        int actualResult = this.dbCtx.Drivers.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }
}
