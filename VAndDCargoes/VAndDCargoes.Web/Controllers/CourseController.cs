using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Services.Models;
using VAndDCargoes.Web.ViewModels.Course;
using static VAndDCargoes.Common.NotificationMessagesConstants;

namespace VAndDCargoes.Web.Controllers;

public class CourseController : BaseController
{
    private readonly IDriverService driverService;
    private readonly ITruckService truckService;
    private readonly ITrailerService trailerService;
    private readonly ICargoService cargoService;
    private readonly ICourseService courseService;

    public CourseController(IDriverService driverService, ITruckService truckService, ITrailerService trailerService, ICargoService cargoService, ICourseService courseService)
    {
        this.driverService = driverService;
        this.truckService = truckService;
        this.trailerService = trailerService;
        this.cargoService = cargoService;
        this.courseService = courseService;
    }

    [HttpGet]
    public async Task<IActionResult> Start()
    {
        string userId = this.GetUserId();

        if (!await this.driverService.IsTheUseAlreadyDriverByIdAsync(userId))
        {
            TempData[ErrorMessage] = "You need to become a driver in order to start a course!";
            return RedirectToAction("Index", "Home");
        }

        if (await this.truckService.GetAllTrucksDrivenByUserByIdAsync(userId) == null ||
            await this.trailerService.GetAllTrailersDrivenByUserByIdAsync(userId) == null ||
            await this.cargoService.GetAllCargoesDeliveringByUserByIdAsync(userId) == null)
        {
            TempData[ErrorMessage] = "You should have at least one truck, trailer and cargo in order to start a course!";
            return RedirectToAction("Index", "Home");
        }

        StartCourseViewModel model = new StartCourseViewModel()
        {
            Trucks = await this.truckService.GetAllTrucksDrivenByUserByIdAsync(userId),
            Trailers = await this.trailerService.GetAllTrailersDrivenByUserByIdAsync(userId),
            Cargoes = await this.cargoService.GetAllCargoesDeliveringByUserByIdAsync(userId)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Start(StartCourseViewModel model)
    {
        string userId = this.GetUserId();

        if (!this.ModelState.IsValid)
        {
            TempData[ErrorMessage] = "An error occured, while starting the course!";
            return View(model);
        }

        if (!await this.courseService.IsTheTruckAvailableAsync(userId, model.TruckId) ||
            !await this.courseService.IsTheTrailerAvailableAsync(userId, model.TrailerId) ||
            !await this.courseService.IsTheCargoAvailable(userId, model.CargoId))
        {
            TempData[ErrorMessage] = "Selected truck, trailer or cargo must be available in order to take a course!";
            return View(model);
        }

        try
        {
            if (!await this.courseService.TakeTheCourseAsync(userId, model))
            {
                TempData[ErrorMessage] = $"Your current truck's condition doesn't allow to start the course! In order to start the course please repair your truck!";
                return View(model);
            }
            
            TempData[SuccessMessage] = $"You successfully took a course from {model.DepartureCity} to {model.ArrivalCity}";
        }
        catch (Exception)
        {
            TempData[ErrorMessage] = "Something happened while starting the course! Please try again later.";
            return this.View(model);
        }

        return RedirectToAction("MyCourses", "Course");
    }

    public async Task<IActionResult> MyCourses()
    {
        string userId = this.GetUserId();

        IEnumerable<AllCoursesViewModel> courses = await this.courseService.GetAllCoursesByUserIdAsync(userId);

        return View(courses);
    }

    public async Task<IActionResult> Finish(string id)
    {
        string userId = this.GetUserId();

        try
        {
            DeleteCourseModel result = await this.courseService.FinishCourseAsync(userId, id);

            await this.cargoService.DeleteCargoByIdAsync(result.CargoId);
            TempData[SuccessMessage] = $"You arrived at your destination and finished the course. Your balance was increased with {result.Reward} EUR.";
        }
        catch (Exception)
        {
            TempData[ErrorMessage] = "Something happend while finishing the course!";
            return RedirectToAction("MyCourses", "Course");
        }

        return RedirectToAction("Index", "Home");
    }
}

