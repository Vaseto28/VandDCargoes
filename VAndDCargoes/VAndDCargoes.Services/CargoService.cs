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
            PhysicalState = (CargoPhysicalState)model.PhysicalState,
            ImageUrl = model.ImageUrl,
            CreatorId = Guid.Parse(userId)
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

    public async Task<Cargo> GetCargoByUserIdAsync(string userId)
    {
        Driver? driver = await this.dbCtx.Drivers.FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            return driver.DriversCargoes.ToList()[0].Cargo;
        }

        return null!;
    }

    public async Task<bool> DeliverCargoByIdAsync(string userId, string cargoId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (await this.dbCtx.Cargoes.FindAsync(Guid.Parse(cargoId)) != null &&
            driver != null)
        {
            DriversCargoes driversCargoes = new DriversCargoes()
            {
                DriverId = driver.Id,
                CargoId = Guid.Parse(cargoId)
            };

            await this.dbCtx.AddAsync(driversCargoes);
            await this.dbCtx.SaveChangesAsync();
            return true;
        }

        return false;
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
            cargo.ImageUrl = model.ImageUrl;
        }

        await this.dbCtx.SaveChangesAsync();
    }

    public async Task<bool> FinishDeliveringOfCargoByIdAsync(string userId, string cargoId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            DriversCargoes? driversCargoes = await this.dbCtx.DriversCargoes
                .FirstOrDefaultAsync(x => x.DriverId.Equals(driver.Id) && x.CargoId.ToString().Equals(cargoId));

            if (driversCargoes != null)
            {
                this.dbCtx.DriversCargoes.Remove(driversCargoes);
                await this.dbCtx.SaveChangesAsync();

                return true;
            }
        }

        return false;
    }

    public async Task<IEnumerable<CargoAllViewModel>> GetAllCargoesAsync(CargoQueryAllViewModel model)
    {
        IQueryable<Cargo> cargoesQuery = this.dbCtx.Cargoes
            .AsQueryable();

        return await this.ModifyTheQuery(cargoesQuery, model);
    }

    public async Task<IEnumerable<CargoAllViewModel>> GetAllCargoesCreatedByUserByIdAsync(CargoQueryAllViewModel queryModel, string userId)
    {
        IQueryable<Cargo> cargoesQuery = this.dbCtx.Cargoes
            .Where(x => x.CreatorId.ToString().Equals(userId))
            .AsQueryable();

        return await this.ModifyTheQuery(cargoesQuery, queryModel);
    }

    public async Task<IEnumerable<CargoAllViewModel>> GetAllCargoesDeliveringByUserByIdAsync(string userId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            IEnumerable<CargoAllViewModel> cargoes = await this.dbCtx.DriversCargoes
            .Where(x => x.Driver.UserId.ToString().Equals(userId))
            .Select(x => new CargoAllViewModel()
            {
                Id = x.Cargo.Id.ToString(),
                Name = x.Cargo.Name,
                Description = x.Cargo.Description,
                ImageUrl = x.Cargo.ImageUrl,
                Weight = x.Cargo.Weight,
                Category = x.Cargo.Category.ToString(),
                PhysicalState = x.Cargo.PhysicalState.ToString()
            })
            .ToArrayAsync();

            if (cargoes.Count() == 0)
            {
                return null!;
            }

            return cargoes;
        }

        return null!;
    }

    public async Task<CargoDetailsViewModel?> GetCargoDetailsById(string id)
    {
        Cargo? cargo = await this.dbCtx.Cargoes
            .FindAsync(Guid.Parse(id));

        if (cargo != null)
        {
            CargoDetailsViewModel cargoDetailsModel = new CargoDetailsViewModel()
            {
                Id = cargo.Id.ToString(),
                Name = cargo.Name,
                Description = cargo.Description,
                Weight = cargo.Weight,
                Category = cargo.Category.ToString(),
                PhysicalState = cargo.PhysicalState.ToString(),
                CreatorName = cargo.Creator.UserName,
                CreatorEmail = cargo.Creator.Email.ToLower(),
                ImageUrl = cargo.ImageUrl
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
                PhysicalState = (int)cargo.PhysicalState,
                ImageUrl = cargo.ImageUrl
            };

            return cargoEditModel;
        }

        return null;
    }

    public async Task<bool> IsCargoStillDelivering(string userId, string cargoeId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));

        if (driver != null)
        {
            DriversCargoes? driversCargoes = await this.dbCtx.DriversCargoes
                .FirstOrDefaultAsync(x => x.DriverId.Equals(driver.Id) && x.CargoId.ToString().Equals(cargoeId));

            if (driversCargoes == null)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsCategoryValid(int categoryNum)
    {
        return categoryNum < CategoryLowerBound || categoryNum > CategoryUpperBound;
    }

    public bool IsPhysicalStateValid(int physicalStateNum)
    {
        return physicalStateNum < PhysicalStateLowerBound || physicalStateNum > PhysicalStateUpperBound;
    }

    private async Task<IEnumerable<CargoAllViewModel>> ModifyTheQuery(IQueryable<Cargo> cargoesQuery, CargoQueryAllViewModel model)
    {
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
                PhysicalState = x.PhysicalState.ToString(),
                ImageUrl = x.ImageUrl
            })
            .ToArrayAsync();

        return cargoes;
    }
}

