using VAndDCargoes.Web.ViewModels.Driver;

namespace VAndDCargoes.Services.Contracts;

public interface IDriverService
{
    Task<bool> IsTheUseAlreadyDriverByIdAsync(string userId);

    Task BecomeDriverAsync(BecomeDriverViewModel model, string userId);
}

