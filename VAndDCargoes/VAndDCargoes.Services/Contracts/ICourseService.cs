using VAndDCargoes.Web.ViewModels.Course;

namespace VAndDCargoes.Services.Contracts;

public interface ICourseService
{
    Task<bool> IsTheTruckAvailableAsync(string userId, string truckId);

    Task<bool> IsTheTrailerAvailableAsync(string userId, string trailerId);

    Task<bool> IsTheCargoAvailable(string userId, string cargoId);

    Task TakeTheCourseAsync(string userId, StartCourseViewModel model);

    Task<IEnumerable<AllCoursesViewModel>> GetAllCoursesByUserIdAsync(string userId);

    Task<decimal> FinishCourseAsync(string userId, string courseId);
}

