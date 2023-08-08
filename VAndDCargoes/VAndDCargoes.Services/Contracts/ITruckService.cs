using VAndDCargoes.Web.ViewModels.Truck;

namespace VAndDCargoes.Services.Contracts;

public interface ITruckService
{
    Task AddTruckAsync(TruckAddViewModel model, string userId);

    Task<IEnumerable<TruckAllViewModel>> GetAllTrucksAsync(TruckQueryAllModel model);

    Task<TruckDetailsViewModel?> GetTruckDetailsByIdAsync(string truckId);

    Task<TruckEditViewModel?> GetTruckByIdForEditAsync(string truckId);

    Task EditTruckAsync(TruckEditViewModel model, string userId);

    Task DeletebyIdAsync(string id);

    bool IsConditionValid(int conditionNum);

    Task<IEnumerable<TruckAllViewModel>> GetAllTrucksCreatedByUserByIdAsync(string id, TruckQueryAllModel queryModel);

    Task<IEnumerable<TruckAllViewModel>> GetAllTrucksDrivenByUserByIdAsync(string id);

    Task<bool> DriveTruckByIdAsync(string userId, string truckId);

    Task<bool> ReleaseTruckByIdAsync(string userId, string truckId);

    Task<bool> IsUserAlreadyDrivingTruckByIdAsync(string userId, string truckId);
}

