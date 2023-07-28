using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Trailer;

namespace VAndDCargoes.Services;

public class TrailerService : ITrailerService
{
    private readonly VAndDCargoesDbContext dbCtx;

    public TrailerService(VAndDCargoesDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
    }

    public async Task AddTrailerAsync(TrailerAddViewModel model)
    {
        Trailer trailer = new Trailer()
        {
            Capacity = model.Capacity,
            Condition = (TrailerCondition)model.Condition,
            Category = (TrailerCategory)model.Category,
            Dementions = (TrailerDemention)model.Dementions
        };

        await this.dbCtx.Trailers.AddAsync(trailer);
        await this.dbCtx.SaveChangesAsync();
    }
}

