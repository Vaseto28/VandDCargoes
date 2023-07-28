using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Truck;
using VAndDCargoes.Web.ViewModels.Truck.Enumerations;

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
            CreatorId = Guid.Parse(userId)
        };

        await this.dbCtx.Trucks.AddAsync(truck);
        await this.dbCtx.SaveChangesAsync();
    }

    public async Task EditTruckAsync(TruckEditViewModel model, string id)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FirstOrDefaultAsync(x => x.Id.ToString() == id);

        if (truck != null)
        {
            truck.Make = model.Make;
            truck.Model = model.Model;
            truck.RegistrationNumber = model.RegistrationNumber;
            truck.TraveledDistance = model.TravelledDistance;
            truck.FuelCapacity = model.FuelCapacity;
            truck.Condition = (TruckCondition)model.Condition;
            truck.CreatedOn = DateTime.Parse(model.CreatedOn);
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
            TrucksOrdering.FreeToDriveFirst => trucksQuery
                .OrderBy(x => x.DriverId != null)
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
                CreatorEmail = x.Creator.Email.ToLower()
            })
            .ToArrayAsync();

        return allTrucksModel;
    }

    public async Task<TruckEditViewModel?> GetTruckByIdForEditAsync(string id)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(id));

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
                CreatedOn = truck.CreatedOn.ToString("dd/MM/yyyy")
            };

            return truckEdit;
        }

        return null;
    }

    public async Task<TruckDetailsViewModel?> GetTruckDetailsByIdAsync(string id)
    {
        Truck? truck = await this.dbCtx.Trucks
            .FirstOrDefaultAsync(x => x.Id.ToString() == id);

        if (truck != null)
        {
            string condition = string.Empty;
            switch ((int)truck.Condition)
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

            TruckDetailsViewModel? truckDetails = new TruckDetailsViewModel()
            {
                Id = truck.Id.ToString(),
                Make = truck.Make,
                Model = truck.Model,
                RegistrationNumber = truck.RegistrationNumber,
                TravelledDistance = truck.TraveledDistance,
                FuelCapacity = truck.FuelCapacity,
                Condition = condition,
                CreatedOn = truck.CreatedOn.ToString("dd/MM/yyyy"),
                CreatorName = truck.Creator.UserName,
                DriverName = truck.Driver != null ?
                    $"{truck.Driver.FirstName} {truck.Driver.SecondName} {truck.Driver.LastName}" :
                    $"This truck hasn't a driver yet!",
                Trailer = truck.Trailer != null ? "Yes" : "No",
                CreatorEmail = truck.Creator.Email.ToLower()
            };

            return truckDetails;
        }

        return null;
    }
}

