using VAndDCargoes.Web.ViewModels.Driver;

namespace VAndDCargoes.Services.Contracts;

public interface IDriverService
{
    Task<bool> IsTheUseAlreadyDriverByIdAsync(string userId);

    Task BecomeDriverAsync(BecomeDriverViewModel model, string userId);

    Task<string> GetTheFullNameOfDriverByUserIdAsync(string userId);

    Task<decimal> GetDriverBalance(string userId);
}

