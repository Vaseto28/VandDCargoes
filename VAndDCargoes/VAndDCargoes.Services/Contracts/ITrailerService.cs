using VAndDCargoes.Web.ViewModels.Trailer;

namespace VAndDCargoes.Services.Contracts;

public interface ITrailerService
{
    Task AddTrailerAsync(TrailerAddViewModel model);
}

