using VAndDCargoes.Web.ViewModels.Repairment;

namespace VAndDCargoes.Services.Contracts;

public interface IRepairmentService
{
    Task RepairTruckAsync(string userId, CreateRepairmentViewModel model);

    Task RepairTrailerAsync(string userId, CreateRepairmentOfTrailerViewModel model);

    Task<bool> IsTruckConditionValidAsync(string truckId);

    Task<bool> IsTrailerConditionValidAsync(string trailerId);

    Task<IEnumerable<AllRepairmentsViewModel>> GetAllRepairmentsByUserIdAsync(string userId);

    int CalculateTheCostOfRepairmanetAsync(int type, int quantity);
}

