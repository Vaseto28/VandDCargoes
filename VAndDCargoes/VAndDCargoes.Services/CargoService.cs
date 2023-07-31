using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Cargo;

namespace VAndDCargoes.Services;

public class CargoService : ICargoService
{
    private readonly VAndDCargoesDbContext dbCtx;

    public CargoService(VAndDCargoesDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
    }

    public async Task AddCargoAsync(CargoAddViewModel model, string userId)
    {
        Cargo cargo = new Cargo()
        {
            Name = model.Name,
            Description = model.Description,
            Weight = model.Weight,
            Category = (CargoCategory)model.Category,
            PhysicalState = (CargoPhysicalState)model.PhysicalState
        };

        await this.dbCtx.Cargoes.AddAsync(cargo);
        await this.dbCtx.SaveChangesAsync();
    }

    public async Task<IEnumerable<CargoAllViewModel>> GetAllCargoesAsync(CargoQueryAllViewModel model)
    {
        IQueryable<Cargo> cargoesQuery = this.dbCtx.Cargoes
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(model.Keyword))
        {
            string wildCard = $"%{model.Keyword.ToLower()}%";

            cargoesQuery = cargoesQuery
                .Where(x => EF.Functions.Like(x.Name, wildCard) ||
                            EF.Functions.Like(x.Description, wildCard));
        }

        cargoesQuery = model.CargoesOrdering switch
        {
            CargoesOrdering.NameAscending => cargoesQuery
                .OrderBy(x => x.Name),
            CargoesOrdering.NameDescending => cargoesQuery
                .OrderByDescending(x => x.Name),
            CargoesOrdering.DescriptionLengthAscending => cargoesQuery
                .OrderBy(x => x.Description),
            CargoesOrdering.DescriptionLengthDescending => cargoesQuery
                .OrderByDescending(x => x.Description),
            CargoesOrdering.WeightAscending => cargoesQuery
                .OrderBy(x => x.Weight),
            CargoesOrdering.WeightDescending => cargoesQuery
            .OrderByDescending(x => x.Weight),
            _ => throw new NotImplementedException()
        };

        IEnumerable<CargoAllViewModel> cargoes = await cargoesQuery
            .Skip((model.CurrentPage - 1) * model.CargoesPerPage)
            .Take(model.CargoesPerPage)
            .Select(x => new CargoAllViewModel()
            {
                Id = x.Id.ToString(),
                Name = x.Name,
                Description = x.Description,
                Weight = x.Weight,
                Category = x.Category.ToString(),
                PhysicalState = x.PhysicalState.ToString()
            })
            .ToArrayAsync();

        return cargoes;
    }
}

