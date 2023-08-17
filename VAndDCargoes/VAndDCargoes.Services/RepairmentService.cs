using Microsoft.EntityFrameworkCore;
using VAndDCargoes.Data;
using VAndDCargoes.Data.Models;
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

    public async Task<IEnumerable<AllRepairmentsViewModel>> GetAllRepairmentsByUserIdAsync(string userId)
    {
        return await this.dbContext.Repairments
            .Where(x => x.MechanicId.ToString().Equals(userId))
            .Select(x => new AllRepairmentsViewModel()
            {
                Id = x.Id,
                Type = x.Type,
                Description = x.Description,
                Cost = x.Cost,
                TruckId = x.MadeOn
            })
            .ToArrayAsync();
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
            Type = model.Type,
            Description = model.Description,
            Cost = model.Cost,
            MadeOn = truck!.Make + " " + truck.Model,
            MechanicId = Guid.Parse(mechanicId)
        };

        await this.dbContext.Repairments.AddAsync(repairment);
        await this.dbContext.SaveChangesAsync();
    }
}

