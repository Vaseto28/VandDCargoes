using VAndDCargoes.Web.ViewModels.Repairment;

namespace VAndDCargoes.Services.Contracts;

public interface IRepairmentService
{
    Task RepairTruckAsync(string userId, CreateRepairmentViewModel model);

    Task<bool> IsTruckConditionValidAsync(string truckId);

    Task<IEnumerable<AllRepairmentsViewModel>> GetAllRepairmentsByUserIdAsync(string userId);
}

