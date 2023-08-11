using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Trailer;
using static VAndDCargoes.Services.Tests.DbSeeder;

namespace VAndDCargoes.Services.Tests;

public class TrailerServiceTests
{
    private DbContextOptions<VAndDCargoesDbContext> options;
    private VAndDCargoesDbContext dbCtx;

    private ITrailerService trailerService;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        this.options = new DbContextOptionsBuilder<VAndDCargoesDbContext>()
                .UseInMemoryDatabase("VAndDCargoesInMemory" + Guid.NewGuid().ToString())
                .Options;

        this.dbCtx = new VAndDCargoesDbContext(this.options);

        this.dbCtx.Database.EnsureCreated();
        SeedDb(this.dbCtx);

        this.trailerService = new TrailerService(this.dbCtx);
    }

    [Test]
    public async Task AddTrailerAsyncShouldCreateNewTrailerAndAddItToTheDbIfDataIsValid()
    {
        await this.trailerService.AddTrailerAsync(DbSeeder.TrailerAddViewModel, DbSeeder.Driver.UserId.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 1;
        int actualResult = this.dbCtx.Trailers.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeleteTrailerByIdAsyncShouldRemoveTrailerFromTheDbIfExists()
    {
        await this.trailerService.DeleteTrailerByIdAsync(this.dbCtx.Trailers.ToList()[0].Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 0;
        int actualResult = this.dbCtx.Trailers.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DeleteTrailerByIdAsyncShouldDoNothingIfDoesNotExist()
    {
        await this.trailerService.DeleteTrailerByIdAsync("dcbc60f7-1a96-406c-9b63-9bece446e62f");
        await this.dbCtx.SaveChangesAsync();

        int expectedResult = 1;
        int actualResult = this.dbCtx.Trailers.Count();

        Assert.That(expectedResult.Equals(actualResult));
    }

    [Test]
    public async Task DriveTrailerByIdASyncShouldAddTheTrailerToTheDriversTrailersIfBothExists()
    {
        await this.trailerService.AddTrailerAsync(DbSeeder.TrailerAddViewModel, DbSeeder.Driver.UserId.ToString());
        bool result = await this.trailerService.DriveTrailerByIdASync(DbSeeder.Driver.UserId.ToString(), this.dbCtx.Trailers.ToList()[0].Id.ToString());

        int expectedResult = 1;
        int actualResult = DbSeeder.Driver.DriversTrailers.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsTrue(result);
    }

    [Test]
    public async Task EditTrailerAsyncShouldUpdateAnExistingTrailerWithNewData()
    {
        await this.trailerService.EditTrailerAsync(DbSeeder.TrailerEditViewModel, this.dbCtx.Trailers.ToList()[0].Id.ToString());
        await this.dbCtx.SaveChangesAsync();

        int expectedCapacity = 20;
        int actualCapacity = this.dbCtx.Trailers.ToList()[0].Capacity;

        int expectedCategory = 1;
        int actualCategory = (int)this.dbCtx.Trailers.ToList()[0].Category;

        int expectedCondition = 2;
        int actualCondition = (int)this.dbCtx.Trailers.ToList()[0].Condition;

        Assert.That(expectedCapacity.Equals(actualCapacity));
        Assert.That(expectedCategory.Equals(actualCategory));
        Assert.That(expectedCondition.Equals(actualCondition));
    }

    [Test]
    public async Task EditTrailerAsyncShouldDoNothingIfTrailerDoesNotExist()
    {
        await this.trailerService.EditTrailerAsync(DbSeeder.TrailerEditViewModel, "5122722e-10f2-4cd6-a1a1-2f3b05a181c2");
        await this.dbCtx.SaveChangesAsync();

        int expectedCapacity = 25;
        int actualCapacity = this.dbCtx.Trailers.ToList()[0].Capacity;

        int expectedCategory = 2;
        int actualCategory = (int)this.dbCtx.Trailers.ToList()[0].Category;

        int expectedCondition = 1;
        int actualCondition = (int)this.dbCtx.Trailers.ToList()[0].Condition;

        Assert.That(expectedCapacity.Equals(actualCapacity));
        Assert.That(expectedCategory.Equals(actualCategory));
        Assert.That(expectedCondition.Equals(actualCondition));
    }

    [Test]
    public async Task GetTrailerDetailsByIdAsyncShouldReturnTrailersDetailsIfExists()
    {
        TrailerDetailsViewModel expectedResult = new TrailerDetailsViewModel()
        {
            Id = this.dbCtx.Trailers.ToList()[0].Id.ToString(),
            Capacity = this.dbCtx.Trailers.ToList()[0].Capacity,
            Category = this.dbCtx.Trailers.ToList()[0].Category.ToString(),
            Condition = this.dbCtx.Trailers.ToList()[0].Condition.ToString(),
            Dementions = this.dbCtx.Trailers.ToList()[0].Dementions.ToString(),
            CreatorEmail = this.dbCtx.Trailers.ToList()[0].Creator.Email,
            ImageUrl = this.dbCtx.Trailers.ToList()[0].ImageUrl
        };

        TrailerDetailsViewModel actualResult = await this.trailerService.GetTrailerDetailsByIdAsync(this.dbCtx.Trailers.ToList()[0].Id.ToString());

        Assert.That(expectedResult.Id.Equals(actualResult.Id));
        Assert.That(expectedResult.Capacity.Equals(actualResult.Capacity));
        Assert.That(expectedResult.Category.Equals(actualResult.Category));
        Assert.That(expectedResult.Condition.Equals(actualResult.Condition));
        Assert.That(expectedResult.Dementions.Equals(actualResult.Dementions));
        Assert.That(expectedResult.ImageUrl.Equals(actualResult.ImageUrl));
    }

    [Test]
    public async Task GetTrailerDetailsByIdAsyncShouldReturnNullIfDoesNotExist()
    {
        TrailerDetailsViewModel actualResult = await this.trailerService.GetTrailerDetailsByIdAsync("66fa318a-29cc-48d9-b48e-85d2f9585334");

        Assert.That(actualResult == null);
    }

    [Test]
    public async Task GetTrailerForEditByIdAsyncShouldReturnTrailerToEditIfExists()
    {
        TrailerEditViewModel expectedResult = new TrailerEditViewModel()
        {
            Capacity = this.dbCtx.Trailers.ToList()[0].Capacity,
            Category = (int)this.dbCtx.Trailers.ToList()[0].Category,
            Condition = (int)this.dbCtx.Trailers.ToList()[0].Condition,
            Dementions = (int)this.dbCtx.Trailers.ToList()[0].Dementions,
            ImageUrl = this.dbCtx.Trailers.ToList()[0].ImageUrl
        };

        TrailerEditViewModel actualResult = await this.trailerService.GetTrailerForEditByIdAsync(this.dbCtx.Trailers.ToList()[0].Id.ToString());

        Assert.That(expectedResult.Capacity.Equals(actualResult.Capacity));
        Assert.That(expectedResult.Category.Equals(actualResult.Category));
        Assert.That(expectedResult.Condition.Equals(actualResult.Condition));
        Assert.That(expectedResult.Dementions.Equals(actualResult.Dementions));
        Assert.That(expectedResult.ImageUrl.Equals(actualResult.ImageUrl));
    }

    [Test]
    public async Task GetTrailerForEditByIdAsyncShouldNullIfDoesNotExist()
    {
        TrailerEditViewModel actualResult = await this.trailerService.GetTrailerForEditByIdAsync("66fa318a-29cc-48d9-b48e-85d2f9585334");

        Assert.That(actualResult == null);
    }

    [Test]
    public void IsCategoryValidShouldReturnFalseIfCategoryNumValid()
    {
        bool result = this.trailerService.IsCategoryValid(2);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsCategoryValidShouldReturnTrueIfCategoryNumIsInvalid()
    {
        bool result = this.trailerService.IsCategoryValid(222);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsConditionValidShouldReturnFalseIfConditionNumValid()
    {
        bool result = this.trailerService.IsConditionValid(2);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsConditionValidShouldReturnTrueIfConditionNumIsInvalid()
    {
        bool result = this.trailerService.IsConditionValid(222);

        Assert.IsTrue(result);
    }

    [Test]
    public void IsDementionsValidShouldReturnFalseIfDementionsNumValid()
    {
        bool result = this.trailerService.IsDementionsValid(2);

        Assert.IsFalse(result);
    }

    [Test]
    public void IsDementionsValidShouldReturnTrueIfDementionsNumIsInvalid()
    {
        bool result = this.trailerService.IsDementionsValid(222);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task IsUserAlreadyDrivingTrailerByIdAsyncShouldReturnFalseIfTheDriverIsStillDrivingTheTrailerIfBothExists()
    {
        bool result = await this.trailerService.IsUserAlreadyDrivingTrailerByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Driver.DriversTrailers.ToList()[0].TrailerId.ToString());

        Assert.IsFalse(result);
    }

    [Test]
    public async Task IsUserAlreadyDrivingTrailerByIdAsyncShouldReturnFalseIfTheDriverIsStillDrivingTheTrailerIfDriverDoesNotExist()
    {
        bool result = await this.trailerService.IsUserAlreadyDrivingTrailerByIdAsync("4feec88a-3449-4789-9268-3d80c7a44c00", DbSeeder.Driver.DriversTrailers.ToList()[0].TrailerId.ToString());

        Assert.IsFalse(result);
    }

    [Test]
    public async Task IsUserAlreadyDrivingTrailerByIdAsyncShouldReturnTrueIfTheDriverIsStillDrivingTheTrailerIfTrailerDoesNotExist()
    {
        bool result = await this.trailerService.IsUserAlreadyDrivingTrailerByIdAsync(DbSeeder.Driver.UserId.ToString(), "4feec88a-3449-4789-9268-3d80c7a44c00");

        Assert.IsTrue(result);
    }

    [Test]
    public async Task ReleaseTrailerByIdAsyncShouldRemoveTheTrailerFromTheDriversDrivenTrailersIfBothExists()
    {
        bool result = await this.trailerService.ReleaseTrailerByIdAsync(DbSeeder.Driver.UserId.ToString(), DbSeeder.Driver.DriversTrailers.ToList()[0].TrailerId.ToString());

        int expectedResult = 0;
        int actualResult = DbSeeder.Driver.DriversTrailers.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsTrue(result);
    }

    [Test]
    public async Task ReleaseTrailerByIdAsyncShouldDoNothingIfDriverDoesNotExist()
    {
        bool result = await this.trailerService.ReleaseTrailerByIdAsync("c3c92fcf-ecb6-4e3c-8615-b6faf656306c", DbSeeder.Driver.DriversTrailers.ToList()[0].TrailerId.ToString());

        int expectedResult = 1;
        int actualResult = DbSeeder.Driver.DriversTrailers.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsFalse(result);
    }

    [Test]
    public async Task ReleaseTrailerByIdAsyncShouldDoNothingIfTrailerDoesNotExist()
    {
        bool result = await this.trailerService.ReleaseTrailerByIdAsync(DbSeeder.Driver.UserId.ToString(), "1997fea6-c145-414b-aca8-82c789bac095");

        int expectedResult = 1;
        int actualResult = DbSeeder.Driver.DriversTrailers.ToList().Count;

        Assert.That(expectedResult.Equals(actualResult));
        Assert.IsFalse(result);
    }

    [Test]
    public async Task GetAllTrailersAsyncShouldReturnAllTrailersInTheDb()
    {
        IEnumerable<TrailerAllViewModel> expectedResult = await this.dbCtx.Trailers
            .Select(x => new TrailerAllViewModel()
            {
                Id = x.Id.ToString(),
                Capacity = x.Capacity,
                Category = x.Category.ToString(),
                Condition = x.Condition.ToString(),
                Dementions = x.Dementions.ToString(),
                ImageUrl = x.ImageUrl
            })
            .ToArrayAsync();

        IEnumerable<TrailerAllViewModel> actualResult = await this.trailerService.GetAllTrailersAsync(new TrailerQueryAllViewModel());

        Assert.That(expectedResult.Count().Equals(actualResult.Count()));
    }
}

