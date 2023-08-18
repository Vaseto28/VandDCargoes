using VAndDCargoes.Services.Models;
using VAndDCargoes.Web.ViewModels.Course;

namespace VAndDCargoes.Services.Contracts;

public interface ICourseService
{
    Task<bool> IsTruckAvailableByIdAsync(string userId, string truckId);

    Task<bool> IsTrailerAvailableByIdAsync(string userId, string trailerId);

    Task<bool> IsCargoAvailableByIdAsync(string userId, string cargoId);

    Task<bool> TakeTheCourseAsync(string userId, StartCourseViewModel model);

    Task<IEnumerable<AllCoursesViewModel>> GetAllCoursesByUserIdAsync(string userId);

    Task<DeleteCourseModel> FinishCourseAsync(string userId, string courseId);
}

