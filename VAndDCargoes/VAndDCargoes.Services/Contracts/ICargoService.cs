using VAndDCargoes.Data.Models;
using VAndDCargoes.Web.ViewModels.Cargo;

namespace VAndDCargoes.Services.Contracts;

public interface ICargoService
{
    Task AddCargoAsync(CargoAddViewModel model, string userId);

    Task<IEnumerable<CargoAllViewModel>> GetAllCargoesAsync(CargoQueryAllViewModel model);

    Task<CargoDetailsViewModel?> GetCargoDetailsById(string id);

    Task<CargoEditViewModel?> GetCargoForEditByIdAsync(string id);

    Task EditCargoAsync(CargoEditViewModel model, string id);

    Task DeleteCargoByIdAsync(string id);

    bool IsCategoryValid(int categoryNum);

    bool IsPhysicalStateValid(int physicalStateNum);

    Task<IEnumerable<CargoAllViewModel>> GetAllCargoesCreatedByUserByIdAsync(CargoQueryAllViewModel queryMode, string userId);

    Task<IEnumerable<CargoAllViewModel>> GetAllCargoesDeliveringByUserByIdAsync(string userId);

    Task<bool> DeliverCargoByIdAsync(string userId, string cargoId);

    Task<bool> FinishDeliveringOfCargoByIdAsync(string userId, string cargoId);

    Task<bool> IsCargoStillDelivering(string userId, string cargoeId);
}

