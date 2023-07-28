using VAndDCargoes.Web.ViewModels.Truck;

namespace VAndDCargoes.Services.Contracts;

public interface ITruckService
{
    Task AddTruckAsync(TruckAddViewModel model, string userId);

    Task<IEnumerable<TruckAllViewModel>> GetAllTrucksAsync(TruckQueryAllModel model);

    Task<TruckDetailsViewModel?> GetTruckDetailsByIdAsync(string id);

    Task<TruckEditViewModel?> GetTruckByIdForEditAsync(string id);

    Task EditTruckAsync(TruckEditViewModel model, string id);
}

