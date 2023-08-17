using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Data.Models.Enumerations;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Repairment;

namespace VAndDCargoes.Services;

public class RepairmentService : IRepairmentService
{
    private readonly VAndDCargoesDbContext dbContext;

    public RepairmentService(VAndDCargoesDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public int CalculateTheCostOfRepairmanetAsync(int type, int quantity)
    {
        int pricePerOne = 0;
        switch (type)
        {
            case 0:
                pricePerOne = 200;
                break;
            case 1:
                pricePerOne = 250;
                break;
            case 2:
                pricePerOne = 50;
                break;
            case 3:
                pricePerOne = 50;
                break;
            case 4:
                pricePerOne = 75;
                break;
            case 5:
                pricePerOne = 90;
                break;
            case 6:
                pricePerOne = 90;
                break;
            case 7:
                pricePerOne = 80;
                break;
            case 8:
                pricePerOne = 470;
                break;
            case 9:
                pricePerOne = 500;
                break;
            case 10:
                pricePerOne = 1200;
                break;
            case 11:
                pricePerOne = 2480;
                break;
            case 12:
                pricePerOne = 25;
                break;
            case 13:
                pricePerOne = 15;
                break;
            case 14:
                pricePerOne = 300;
                break;
            case 15:
                pricePerOne = 320;
                break;
            case 16:
                pricePerOne = 700;
                break;
            case 17:
                pricePerOne = 520;
                break;
            case 18:
                pricePerOne = 490;
                break;
            case 19:
                pricePerOne = 50;
                break;
            case 20:
                pricePerOne = 60;
                break;
            case 21:
                pricePerOne = 90;
                break;
            case 22:
                pricePerOne = 110;
                break;
        }

        return quantity * pricePerOne;
    }

    public async Task<IEnumerable<AllRepairmentsViewModel>> GetAllRepairmentsByUserIdAsync(string userId)
    {
        return await this.dbContext.Repairments
            .Where(x => x.MechanicId.ToString().Equals(userId))
            .Select(x => new AllRepairmentsViewModel()
            {
                Id = x.Id,
                Type = (int)x.Type,
                Description = x.Description,
                Quantity = x.Quantity,
                Cost = x.Cost,
                TruckId = x.MadeOn
            })
            .ToArrayAsync();
    }

    public async Task<bool> IsTrailerConditionValidAsync(string trailerId)
    {
        Trailer? trailer = await this.dbContext.Trailers
            .FirstOrDefaultAsync(x => x.Id.ToString().Equals(trailerId));

        if (trailer != null)
        {
            if (trailer.Condition >= TrailerCondition.NeedOfService)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public async Task<bool> IsTruckConditionValidAsync(string truckId)
    {
        Truck? truck = await this.dbContext.Trucks
            .FindAsync(Guid.Parse(truckId));

        if (truck != null && (int)truck.Condition > 0)
        {
            return true;
        }

        return false;
    }

    public async Task RepairTrailerAsync(string mechanicId, CreateRepairmentOfTrailerViewModel model)
    {
        Trailer? trailer = await this.dbContext.Trailers
            .FindAsync(model.TrailerId);

        if (trailer != null)
        {
            trailer.Condition -= 1;
        }

        Repairment repairment = new Repairment()
        {
            Type = (RepairmentAvailableTypes)model.Type,
            Description = model.Description,
            Quantity = model.Quantity,
            Cost = model.Cost,
            MadeOn = trailer!.Category.ToString(),
            MechanicId = Guid.Parse(mechanicId)
        };

        await this.dbContext.Repairments.AddAsync(repairment);
        await this.dbContext.SaveChangesAsync();
    }

    public async Task RepairTruckAsync(string mechanicId, CreateRepairmentViewModel model)
    {
        Truck? truck = await this.dbContext.Trucks
            .FindAsync(model.TruckId);

        if (truck != null)
        {
            truck.Condition -= 1;
        }

        Repairment repairment = new Repairment()
        {
            Type = (RepairmentAvailableTypes)model.Type,
            Description = model.Description,
            Quantity = model.Quantity,
            Cost = model.Cost,
            MadeOn = truck!.Make + " " + truck.Model,
            MechanicId = Guid.Parse(mechanicId)
        };

        await this.dbContext.Repairments.AddAsync(repairment);
        await this.dbContext.SaveChangesAsync();
    }
}

