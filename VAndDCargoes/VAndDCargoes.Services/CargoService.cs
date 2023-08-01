using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Cargo;
using static VAndDCargoes.Common.EntitiesValidations.Cargo;

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

    public async Task DeleteCargoByIdAsync(string id)
    {
        Cargo? cargo = await this.dbCtx.Cargoes
            .FindAsync(Guid.Parse(id));

        if (cargo != null)
        {
            this.dbCtx.Remove(cargo);
            await this.dbCtx.SaveChangesAsync();
        }
    }

    public async Task EditCargoAsync(CargoEditViewModel model, string id)
    {
        Cargo? cargo = await this.dbCtx.Cargoes
            .FindAsync(Guid.Parse(id));

        if (cargo != null)
        {
            cargo.Name = model.Name;
            cargo.Description = model.Description;
            cargo.Weight = model.Weight;
            cargo.Category = (CargoCategory)model.Category;
            cargo.PhysicalState = (CargoPhysicalState)model.PhysicalState;
        }

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

    public async Task<CargoDetailsViewModel?> GetCargoDetailsById(string id)
    {
        Cargo? cargo = await this.dbCtx.Cargoes
            .FindAsync(Guid.Parse(id));

        if (cargo != null)
        {
            string category = string.Empty;
            switch ((int)cargo.Category)
            {
                case 0:
                    category = "Food";
                    break;
                case 1:
                    category = "Livestock";
                    break;
                case 2:
                    category = "Chemicals";
                    break;
                case 3:
                    category = "Flamebal substances";
                    break;
                case 4:
                    category = "Cars";
                    break;
                case 5:
                    category = "Equipment or machinery";
                    break;
                case 6:
                    category = "Dry bulk cargo";
                    break;
            }

            CargoDetailsViewModel cargoDetailsModel = new CargoDetailsViewModel()
            {
                Id = cargo.Id.ToString(),
                Name = cargo.Name,
                Description = cargo.Description,
                Weight = cargo.Weight,
                Category = category,
                PhysicalState = cargo.PhysicalState.ToString(),
                CreatorName = cargo.Creator.UserName,
                CreatorEmail = cargo.Creator.Email.ToLower()
            };

            return cargoDetailsModel;
        }

        return null;
    }

    public async Task<CargoEditViewModel?> GetCargoForEditByIdAsync(string id)
    {
        Cargo? cargo = await this.dbCtx.Cargoes
            .FindAsync(Guid.Parse(id));

        if (cargo != null)
        {
            CargoEditViewModel cargoEditModel = new CargoEditViewModel()
            {
                Name = cargo.Name,
                Description = cargo.Description,
                Weight = cargo.Weight,
                Category = (int)cargo.Category,
                PhysicalState = (int)cargo.PhysicalState
            };

            return cargoEditModel;
        }

        return null;
    }

    public bool IsCategoryValid(int categoryNum)
    {
        return categoryNum < CategoryLowerBound || categoryNum > CategoryUpperBound;
    }

    public bool IsPhysicalStateValid(int physicalStateNum)
    {
        return physicalStateNum < PhysicalStateLowerBound || physicalStateNum > PhysicalStateUpperBound;
    }
}

