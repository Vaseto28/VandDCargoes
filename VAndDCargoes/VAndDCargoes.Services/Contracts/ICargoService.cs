using VAndDCargoes.Web.ViewModels.Cargo;

namespace VAndDCargoes.Services.Contracts;

public interface ICargoService
{
    Task AddCargoAsync(CargoAddViewModel model, string userId);

    Task<IEnumerable<CargoAllViewModel>> GetAllCargoesAsync(CargoQueryAllViewModel model);
}

