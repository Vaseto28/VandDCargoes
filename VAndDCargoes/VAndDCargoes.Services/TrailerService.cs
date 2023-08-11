using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Trailer;
using VAndDCargoes.Web.ViewModels.Trailer.Enumerations;
using static VAndDCargoes.Common.EntitiesValidations.Trailer;


namespace VAndDCargoes.Services;

public class TrailerService : ITrailerService
{
    private readonly VAndDCargoesDbContext dbCtx;

    public TrailerService(VAndDCargoesDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
    }

    public async Task AddTrailerAsync(TrailerAddViewModel model, string userId)
    {
        Trailer trailer = new Trailer()
        {
            Capacity = model.Capacity,
            Condition = (TrailerCondition)model.Condition,
            Category = (TrailerCategory)model.Category,
            Dementions = (TrailerDemention)model.Dementions,
            CreatorId = Guid.Parse(userId),
            ImageUrl = model.ImageUrl
        };

        await this.dbCtx.Trailers.AddAsync(trailer);
        await this.dbCtx.SaveChangesAsync();
    }

    public async Task DeleteTrailerByIdAsync(string id)
    {
        Trailer? trailer = await this.dbCtx.Trailers
            .FindAsync(Guid.Parse(id));

        if (trailer != null)
        {
            this.dbCtx.Trailers.Remove(trailer);
            await this.dbCtx.SaveChangesAsync();
        }
    }

    public async Task<bool> DriveTrailerByIdASync(string userId, string trailerId)
    {
        Driver? driver = await this.dbCtx.Drivers.FirstAsync(x => x.UserId.ToString().Equals(userId));

        if (await this.dbCtx.Trailers.FindAsync(Guid.Parse(trailerId)) != null &&
            driver != null)
        {
            DriversTrailers driversTrailers = new DriversTrailers()
            {
                DriverId = driver.Id,
                TrailerId = Guid.Parse(trailerId)
            };

            await this.dbCtx.DriversTrailers.AddAsync(driversTrailers);
            await this.dbCtx.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task EditTrailerAsync(TrailerEditViewModel model, string trailerId)
    {
        Trailer? trailerToEdit = await this.dbCtx.Trailers
            .FindAsync(Guid.Parse(trailerId));

        if (trailerToEdit != null)
        {
            trailerToEdit.Capacity = model.Capacity;
            trailerToEdit.Category = (TrailerCategory)model.Category;
            trailerToEdit.Condition = (TrailerCondition)model.Condition;
            trailerToEdit.Dementions = (TrailerDemention)model.Dementions;
            trailerToEdit.ImageUrl = model.ImageUrl;
        }

        await this.dbCtx.SaveChangesAsync();
    }

    public async Task<IEnumerable<TrailerAllViewModel>> GetAllTrailersAsync(TrailerQueryAllViewModel queryModel)
    {
        IQueryable<Trailer> trailersQuery = this.dbCtx.Trailers
            .AsQueryable();

        return await this.ModifyTheQuery(trailersQuery, queryModel);
    }

    public async Task<IEnumerable<TrailerAllViewModel>> GetAllTrailersCreatedByUserByIdAsync(string userId, TrailerQueryAllViewModel queryModel)
    {
        IQueryable<Trailer> trailersQuery = this.dbCtx.Trailers
            .Where(x => x.CreatorId.ToString().Equals(userId))
            .AsQueryable();

        return await this.ModifyTheQuery(trailersQuery, queryModel);
    }

    public async Task<IEnumerable<TrailerAllViewModel>> GetAllTrailersDrivenByUserByIdAsync(string userId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            return await this.dbCtx.DriversTrailers
            .Where(x => x.DriverId.Equals(driver.Id))
            .Select(x => new TrailerAllViewModel()
            {
                Id = x.TrailerId.ToString(),
                Capacity = x.Trailer.Capacity,
                Category = x.Trailer.Category.ToString(),
                Condition = x.Trailer.Condition.ToString(),
                Dementions = x.Trailer.Dementions.ToString(),
                ImageUrl = x.Trailer.ImageUrl
            })
            .ToArrayAsync();
        }

        return new List<TrailerAllViewModel>();
    }

    public async Task<TrailerDetailsViewModel?> GetTrailerDetailsByIdAsync(string trailerId)
    {
        Trailer? trailer = await this.dbCtx.Trailers
            .FindAsync(Guid.Parse(trailerId));

        if (trailer != null)
        {
            TrailerDetailsViewModel trailerDetails = new TrailerDetailsViewModel()
            {
                Id = trailer.Id.ToString(),
                Capacity = trailer.Capacity,
                Category = trailer.Category.ToString(),
                Condition = trailer.Condition.ToString(),
                Dementions = trailer.Dementions.ToString(),
                CreatorEmail = trailer.Creator.Email,
                ImageUrl = trailer.ImageUrl
            };

            return trailerDetails;
        }

        return null;
    }

    public async Task<TrailerEditViewModel?> GetTrailerForEditByIdAsync(string trailerId)
    {
        Trailer? trailer = await this.dbCtx.Trailers
            .FindAsync(Guid.Parse(trailerId));

        if (trailer != null)
        {
            TrailerEditViewModel trailerToEdit = new TrailerEditViewModel()
            {
                Capacity = trailer.Capacity,
                Category = (int)trailer.Category,
                Condition = (int)trailer.Condition,
                Dementions = (int)trailer.Dementions,
                ImageUrl = trailer.ImageUrl
            };

            return trailerToEdit;
        }

        return null;
    }

    public bool IsCategoryValid(int categoryNum)
    {
        return categoryNum < CategoryLowerBound || categoryNum > CategoryUpperBound;
    }

    public bool IsConditionValid(int conditionNum)
    {
        return conditionNum < ConditionLowerBound || conditionNum > ConditionUpperBound;
    }

    public bool IsDementionsValid(int dementionsNum)
    {
        return dementionsNum < DementionLowerBound || dementionsNum > DementionUpperBound;
    }

    public async Task<bool> IsUserAlreadyDrivingTrailerByIdAsync(string userId, string trailerId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));
        if (driver != null)
        {
            DriversTrailers? driversTrailers = await this.dbCtx.DriversTrailers
                .FirstOrDefaultAsync(x => x.DriverId.Equals(driver.Id) && x.TrailerId.ToString().Equals(trailerId));

            if (driversTrailers == null)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> ReleaseTrailerByIdAsync(string userId, string trailerId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            DriversTrailers? driversTrailers = await this.dbCtx.DriversTrailers
                .FirstOrDefaultAsync(x => x.DriverId.Equals(driver.Id) && x.TrailerId.ToString().Equals(trailerId));

            if (driversTrailers != null)
            {
                this.dbCtx.DriversTrailers.Remove(driversTrailers);
                await this.dbCtx.SaveChangesAsync();
                return true;
            }
        }

        return false;
    }

    private async Task<IEnumerable<TrailerAllViewModel>> ModifyTheQuery(IQueryable<Trailer> trailersQuery, TrailerQueryAllViewModel queryModel)
    {
        trailersQuery = queryModel.TrailersOrdering switch
        {
            TrailersOrdering.ByCategory => trailersQuery
                .OrderBy(x => (int)x.Category),
            TrailersOrdering.ByCapacityAscending => trailersQuery
                .OrderBy(x => x.Capacity),
            TrailersOrdering.ByCapacityDescending => trailersQuery
                .OrderByDescending(x => x.Capacity),
            TrailersOrdering.ByConditionAscending => trailersQuery
                .OrderBy(x => (int)x.Condition),
            TrailersOrdering.ByConditionDescending => trailersQuery
                .OrderByDescending(x => (int)x.Condition),
            TrailersOrdering.ByDementionAscending => trailersQuery
                .OrderBy(x => (int)x.Dementions),
            TrailersOrdering.ByDementionDescending => trailersQuery
                .OrderByDescending(x => (int)x.Dementions),
            _ => throw new NotImplementedException()
        };

        return await trailersQuery
            .Skip((queryModel.CurrentPage - 1) * queryModel.TrailersPerPage)
            .Take(queryModel.TrailersPerPage)
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
    }
}

