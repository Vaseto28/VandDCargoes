using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Truck;
using VAndDCargoes.Web.ViewModels.Truck.Enumerations;
using static VAndDCargoes.Common.EntitiesValidations.Truck;

namespace VAndDCargoes.Services;

public class TruckService : ITruckService
{
    private readonly VAndDCargoesDbContext dbCtx;

    public TruckService(VAndDCargoesDbContext dbCtx)
    {
        this.dbCtx = dbCtx;
    }

    public async Task AddTruckAsync(TruckAddViewModel model, string userId)
    {
        Truck truck = new Truck()
        {
            Make = model.Make,
            Model = model.Model,
            RegistrationNumber = model.RegistrationNumber,
            FuelCapacity = model.FuelCapacity,
            TraveledDistance = model.TravelledDistance,
            Condition = (TruckCondition)model.Condition,
            CreatorId = Guid.Parse(userId),
            ImageUrl = model.ImageUrl
        };

        await this.dbCtx.Trucks.AddAsync(truck);
        await this.dbCtx.SaveChangesAsync();
    }

    public async Task DeletebyIdAsync(string id)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FindAsync(Guid.Parse(id));

        if (truck != null)
        {
            this.dbCtx.Trucks.Remove(truck);
            await this.dbCtx.SaveChangesAsync();
        }
    }

    public async Task<bool> DriveTruckByIdAsync(string userId, string truckId)
    {
        Driver driver = await this.dbCtx.Drivers.FirstAsync(x => x.UserId.ToString().Equals(userId));

        if (await this.dbCtx.Trucks.FindAsync(Guid.Parse(truckId)) != null &&
            driver != null)
        {
            DriversTrucks driversTrucks = new DriversTrucks()
            {
                DriverId = driver.Id,
                TruckId = Guid.Parse(truckId)
            };

            await this.dbCtx.DriversTrucks.AddAsync(driversTrucks);
            await this.dbCtx.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task EditTruckAsync(TruckEditViewModel model, string truckId)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FindAsync(Guid.Parse(truckId));

        if (truck != null)
        {
            truck.Make = model.Make;
            truck.Model = model.Model;
            truck.RegistrationNumber = model.RegistrationNumber;
            truck.TraveledDistance = model.TravelledDistance;
            truck.FuelCapacity = model.FuelCapacity;
            truck.Condition = (TruckCondition)model.Condition;
            truck.CreatedOn = DateTime.Parse(model.CreatedOn);
            truck.ImageUrl = model.ImageUrl;
        }

        await this.dbCtx.SaveChangesAsync();
    }

    public async Task<IEnumerable<TruckAllViewModel>> GetAllTrucksAsync(TruckQueryAllModel queryModel)
    {
        IQueryable<Truck> trucksQuery = this.dbCtx.Trucks
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryModel.Keyword))
        {
            string wildCard = $"%{queryModel.Keyword.ToLower()}%";

            trucksQuery = trucksQuery
                .Where(x => EF.Functions.Like(x.Make, wildCard) ||
                            EF.Functions.Like(x.Model, wildCard));
        }

        //TODO: Check if this functionality is working

        trucksQuery = queryModel.TrucksOrdering switch
        {
            TrucksOrdering.Newest => trucksQuery
                .OrderByDescending(x => x.CreatedOn),
            TrucksOrdering.Oldest => trucksQuery
                .OrderBy(x => x.CreatedOn),
            TrucksOrdering.MakeAscending => trucksQuery
                .OrderBy(x => x.Make),
            TrucksOrdering.MakeDescending => trucksQuery
                .OrderByDescending(x => x.Make),
            //TrucksOrdering.FreeToDriveFirst => trucksQuery
            //    .OrderBy(x => x.DriverId != null),
            _ => throw new NotImplementedException()
        };

        IEnumerable<TruckAllViewModel> allTrucksModel = await trucksQuery
            .Skip((queryModel.CurrentPage - 1) * queryModel.TrucksPerPage)
            .Take(queryModel.TrucksPerPage)
            .Select(x => new TruckAllViewModel()
            {
                Id = x.Id.ToString(),
                Make = x.Make,
                Model = x.Model,
                FuelCapacity = x.FuelCapacity,
                TravelledDistance = x.TraveledDistance,
                ImageUrl = x.ImageUrl
            })
            .ToArrayAsync();

        return allTrucksModel;
    }

    public async Task<IEnumerable<TruckAllViewModel>> GetAllTrucksCreatedByUserByIdAsync(string id, TruckQueryAllModel queryModel)
    {
        IQueryable<Truck> trucksQuery = this.dbCtx.Trucks
            .Where(x => x.CreatorId.ToString().Equals(id))
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryModel.Keyword))
        {
            string wildCard = $"%{queryModel.Keyword.ToLower()}%";

            trucksQuery = trucksQuery
                .Where(x => EF.Functions.Like(x.Make, wildCard) ||
                            EF.Functions.Like(x.Model, wildCard));
        }

        //TODO: Check if this functionality is working

        trucksQuery = queryModel.TrucksOrdering switch
        {
            TrucksOrdering.Newest => trucksQuery
                .OrderByDescending(x => x.CreatedOn),
            TrucksOrdering.Oldest => trucksQuery
                .OrderBy(x => x.CreatedOn),
            TrucksOrdering.MakeAscending => trucksQuery
                .OrderBy(x => x.Make),
            TrucksOrdering.MakeDescending => trucksQuery
                .OrderByDescending(x => x.Make),
            //TrucksOrdering.FreeToDriveFirst => trucksQuery
            //    .OrderBy(x => x.DriverId != null),
            _ => throw new NotImplementedException()
        };

        IEnumerable<TruckAllViewModel> allTrucksModel = await trucksQuery
            .Skip((queryModel.CurrentPage - 1) * queryModel.TrucksPerPage)
            .Take(queryModel.TrucksPerPage)
            .Select(x => new TruckAllViewModel()
            {
                Id = x.Id.ToString(),
                Make = x.Make,
                Model = x.Model,
                FuelCapacity = x.FuelCapacity,
                TravelledDistance = x.TraveledDistance,
                ImageUrl = x.ImageUrl
            })
            .ToArrayAsync();

        return allTrucksModel;
    }

    public async Task<IEnumerable<TruckAllViewModel>> GetAllTrucksDrivenByUserByIdAsync(string id)
    {
        Driver? driver = await this.dbCtx.Drivers.FirstOrDefaultAsync(x => x.UserId.ToString().Equals(id));

        if (driver != null)
        {
            return await this.dbCtx.DriversTrucks
            .Where(x => x.DriverId.Equals(driver.Id))
            .Select(x => new TruckAllViewModel()
            {
                Id = x.Truck.Id.ToString(),
                Make = x.Truck.Make,
                Model = x.Truck.Model,
                FuelCapacity = x.Truck.FuelCapacity,
                TravelledDistance = x.Truck.TraveledDistance,
                ImageUrl = x.Truck.ImageUrl
            })
            .ToArrayAsync();
        }

        return new List<TruckAllViewModel>();
    }

    public async Task<TruckEditViewModel?> GetTruckByIdForEditAsync(string truckId)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FindAsync(Guid.Parse(truckId));

        if (truck != null)
        {
            TruckEditViewModel truckEdit = new TruckEditViewModel()
            {
                Make = truck.Make,
                Model = truck.Model,
                RegistrationNumber = truck.RegistrationNumber,
                TravelledDistance = truck.TraveledDistance,
                FuelCapacity = truck.FuelCapacity,
                Condition = (int)truck.Condition,
                CreatedOn = truck.CreatedOn.ToString("MM/dd/yyyy"),
                ImageUrl = truck.ImageUrl
            };

            return truckEdit;
        }

        return null;
    }

    public async Task<TruckDetailsViewModel?> GetTruckDetailsByIdAsync(string truckId)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FindAsync(Guid.Parse(truckId));

        if (truck != null)
        {
            TruckDetailsViewModel? truckDetails = new TruckDetailsViewModel()
            {
                Id = truck.Id.ToString(),
                Make = truck.Make,
                Model = truck.Model,
                RegistrationNumber = truck.RegistrationNumber,
                TravelledDistance = truck.TraveledDistance,
                FuelCapacity = truck.FuelCapacity,
                Condition = truck.Condition.ToString(),
                CreatedOn = truck.CreatedOn.ToString("dd/MM/yyyy"),
                CreatorName = truck.Creator.UserName,
                //DriverName = truck.Driver != null ?
                //    $"{truck.Driver.FirstName} {truck.Driver.SecondName} {truck.Driver.LastName}" :
                //    $"This truck hasn't a driver yet!",
                CreatorEmail = truck.Creator.Email.ToLower(),
                ImageUrl = truck.ImageUrl
            };

            return truckDetails;
        }

        return null;
    }

    public bool IsConditionValid(int conditionNum)
    {
        return conditionNum < ConditionLowerBound || conditionNum > ConditionUpperBound;
    }

    public async Task<bool> IsUserAlreadyDrivingTruckByIdAsync(string userId, string truckId)
    {
        Driver? driver = await this.dbCtx.Drivers
            .FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));
        if (driver != null)
        {
            DriversTrucks? driversTrucks = await this.dbCtx.DriversTrucks
                .FirstOrDefaultAsync(x => x.DriverId.Equals(driver.Id) && x.TruckId.ToString().Equals(truckId));

            if (driversTrucks == null)
            {
                return true;
            }
        }

        return false;
    }

    public async Task<bool> ReleaseTruckByIdAsync(string userId, string truckId)
    {
        Driver? driver = await this.dbCtx.Drivers.FirstOrDefaultAsync(x => x.UserId.ToString().Equals(userId));
        if (driver == null)
        {
            return false;
        }

        DriversTrucks? driversTrucks = await this.dbCtx.DriversTrucks
            .FirstOrDefaultAsync(x => x.TruckId.ToString().Equals(truckId));
        if (driversTrucks == null)
        {
            return false;
        }

        this.dbCtx.DriversTrucks.Remove(driversTrucks);
        await this.dbCtx.SaveChangesAsync();
        return true;
    }
}

