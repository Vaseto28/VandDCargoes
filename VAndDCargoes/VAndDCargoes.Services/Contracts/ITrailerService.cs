using VAndDCargoes.Web.ViewModels.Trailer;

namespace VAndDCargoes.Services.Contracts;

public interface ITrailerService
{
    Task AddTrailerAsync(TrailerAddViewModel model, string userId);

    Task<IEnumerable<TrailerAllViewModel>> GetAllTrailersAsync(TrailerQueryAllViewModel queryModel);

    Task<TrailerDetailsViewModel?> GetTrailerDetailsByIdAsync(string trailerId);

    Task<TrailerEditViewModel?> GetTrailerForEditByIdAsync(string trailerId);

    Task EditTrailerAsync(TrailerEditViewModel model, string userId);

    Task DeleteTrailerByIdAsync(string id);

    bool IsCategoryValid(int categoryNum);

    bool IsConditionValid(int conditionNum);

    bool IsDementionsValid(int dementionsNum);

    Task<IEnumerable<TrailerAllViewModel>> GetAllTrailersCreatedByUserByIdAsync(string userId, TrailerQueryAllViewModel queryModel);

    Task<IEnumerable<TrailerAllViewModel>> GetAllTrailersDrivenByUserByIdAsync(string userId);

    Task<bool> DriveTrailerByIdASync(string userId, string trailerId);

    Task<bool> ReleaseTrailerByIdAsync(string userId, string trailerId);

    Task<bool> IsUserAlreadyDrivingTrailerByIdAsync(string userId, string trailerId);
}

