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
}

