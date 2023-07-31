using VAndDCargoes.Web.ViewModels.Truck;

namespace VAndDCargoes.Services.Contracts;

public interface ITruckService
{
    Task AddTruckAsync(TruckAddViewModel model, string userId);

    Task<IEnumerable<TruckAllViewModel>> GetAllTrucksAsync(TruckQueryAllModel model);

    Task<TruckDetailsViewModel?> GetTruckDetailsByIdAsync(string truckId);

    Task<TruckEditViewModel?> GetTruckByIdForEditAsync(string truckId);

    Task EditTruckAsync(TruckEditViewModel model, string userId);
}

