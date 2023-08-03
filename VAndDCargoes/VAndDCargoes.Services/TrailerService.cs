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

        IEnumerable<TrailerAllViewModel> trailers = await trailersQuery
            .Skip((queryModel.CurrentPage - 1) * queryModel.TrailersPerPage)
            .Take(queryModel.TrailersPerPage)
            .Select(x => new TrailerAllViewModel()
            {
                Id = x.Id.ToString(),
                Capacity = x.Capacity,
                Category = x.Category.ToString(),
                Condition = x.Condition.ToString(),
                Dementions = x.Dementions.ToString(),
                Cargo = x.CargoId == null ?
                    "This trailer hasn't cargo yet." :
                    $"{x.Cargo!.Name}",
                ImageUrl = x.ImageUrl
            })
            .ToArrayAsync();

        return trailers;
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
                Cargo = trailer.Cargo == null ?
                    "This trailer hasn't a cargo yet." :
                    $"{trailer.Cargo.Name}",
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
}

