using VAndDCargoes.Web.ViewModels.Driver;

namespace VAndDCargoes.Services.Contracts;

public interface IDriverService
{
    Task<bool> IsTheUserAlreadyDriver(string id);

    Task BecomeDriverAsync(BecomeDriverViewModel model, string userId);
}

