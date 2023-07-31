using System;
using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Trailer;
using VAndDCargoes.Web.ViewModels.Trailer.Enumerations;

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
            CreatorId = Guid.Parse(userId)
        };

        await this.dbCtx.Trailers.AddAsync(trailer);
        await this.dbCtx.SaveChangesAsync();
    }

    public async Task EditTruckAsync(TrailerEditViewModel model, string trailerId)
    {
        Trailer? trailerToEdit = await this.dbCtx.Trailers
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(trailerId));

        if (trailerToEdit != null)
        {
            trailerToEdit.Capacity = model.Capacity;
            trailerToEdit.Category = (TrailerCategory)model.Category;
            trailerToEdit.Condition = (TrailerCondition)model.Condition;
            trailerToEdit.Dementions = (TrailerDemention)model.Dementions;
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
                    $"{x.Cargo!.Name}"
            })
            .ToArrayAsync();

        return trailers;
    }

    public async Task<TrailerDetailsViewModel?> GetTrailerDetailsByIdAsync(string trailerId)
    {
        Trailer? trailer = await this.dbCtx.Trailers
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(trailerId));

        if (trailer != null)
        {
            string category = string.Empty;
            switch ((int)trailer.Category)
            {
                case 0:
                    category = "Platform";
                    break;
                case 1:
                    category = "Refrigerator";
                    break;
                case 2:
                    category = "Gondola";
                    break;
                case 3:
                    category = "Tank truck";
                    break;
                case 4:
                    category = "Container ship";
                    break;
                case 5:
                    category = "Car transporter";
                    break;
            }

            string condition = string.Empty;
            switch ((int)trailer.Condition)
            {
                case 0:
                    condition = "Excellent";
                    break;
                case 1:
                    condition = "Very good";
                    break;
                case 2:
                    condition = "Good";
                    break;
                case 3:
                    condition = "Bad";
                    break;
                case 4:
                    condition = "Need of service";
                    break;
            }

            string dementions = string.Empty;
            switch ((int)trailer.Dementions)
            {
                case 0:
                    dementions = "Small trailer";
                    break;
                case 1:
                    dementions = "Hanger";
                    break;
                case 2:
                    dementions = "Mega trailer";
                    break;
                case 3:
                    dementions = "Jumbo trailer";
                    break;
            }

            TrailerDetailsViewModel trailerDetails = new TrailerDetailsViewModel()
            {
                Id = trailer.Id.ToString(),
                Capacity = trailer.Capacity,
                Category = category,
                Condition = condition,
                Dementions = dementions,
                Cargo = trailer.Cargo == null ?
                    "This trailer hasn't a cargo yet." :
                    $"{trailer.Cargo.Name}",
                CreatorEmail = trailer.Creator.Email
            };

            return trailerDetails;
        }

        return null;
    }

    public async Task<TrailerEditViewModel?> GetTrailerForEditByIdAsync(string trailerId)
    {
        Trailer? trailer = await this.dbCtx.Trailers
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(trailerId));

        if (trailer != null)
        {
            TrailerEditViewModel trailerToEdit = new TrailerEditViewModel()
            {
                Capacity = trailer.Capacity,
                Category = (int)trailer.Category,
                Condition = (int)trailer.Condition,
                Dementions = (int)trailer.Dementions
            };

            return trailerToEdit;
        }

        return null;
    }
}

