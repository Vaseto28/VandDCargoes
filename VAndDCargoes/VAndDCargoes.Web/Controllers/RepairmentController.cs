using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Repairment;
using static VAndDCargoes.Common.NotificationMessagesConstants;

namespace VAndDCargoes.Web.Controllers;

public class RepairmentController : BaseController
{
    private readonly ITrailerService trailerService;
    private readonly ITruckService truckService;
    private readonly IRepairmentService repairmentService;
    private readonly IDriverService driverService;

    public RepairmentController(ITruckService truckService, IRepairmentService repairmentService, IDriverService driverService, ITrailerService trailerService)
    {
        this.truckService = truckService;
        this.repairmentService = repairmentService;
        this.driverService = driverService;
        this.trailerService = trailerService;
    }

    [HttpGet]
    public async Task<IActionResult> Repair()
    {
        string userId = this.GetUserId();

        CreateRepairmentViewModel model = new CreateRepairmentViewModel()
        {
            Trucks = await this.truckService.GetAllTrucksDrivenByUserByIdAsync(userId)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Repair(CreateRepairmentViewModel model)
    {
        if (!await this.repairmentService.IsTruckConditionValidAsync(model.TruckId.ToString()) || !this.ModelState.IsValid)
        {
            TempData[ErrorMessage] = "This truck is in perfect condition!";

            return View(model);
        }

        string mechanicId = this.GetUserId();

        try
        {
            int totalPrice = this.repairmentService.CalculateTheCostOfRepairmanetAsync(model.Type, model.Quantity);
            model.Cost = totalPrice / model.Quantity;
            if (!await this.driverService.SpendForRepairment(mechanicId, totalPrice))
            {
                TempData[ErrorMessage] = $"Insufficient balance!";
                return View(model);
            }
            
            await this.repairmentService.RepairTruckAsync(mechanicId, model);
            TempData[SuccessMessage] = $"You successfully repaired your truck! Total price was: {this.repairmentService.CalculateTheCostOfRepairmanetAsync(model.Type, model.Quantity)} EUR";
        }
        catch (Exception)
        {
            TempData[ErrorMessage] = "Something happened while repairing your truck!";
            return View(model);
        }

        return RedirectToAction("MyRepairments", "Repairment");
    }

    [HttpGet]
    public async Task<IActionResult> RepairTrailer()
    {
        string userId = this.GetUserId();

        CreateRepairmentOfTrailerViewModel model = new CreateRepairmentOfTrailerViewModel()
        {
            Trailers = await this.trailerService.GetAllTrailersDrivenByUserByIdAsync(userId)
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> RepairTrailer(CreateRepairmentOfTrailerViewModel model)
    {
        if (!await this.repairmentService.IsTrailerConditionValidAsync(model.TrailerId.ToString()) || !this.ModelState.IsValid)
        {
            TempData[ErrorMessage] = "This trailer is in perfect condition!";

            return View(model);
        }

        string mechanicId = this.GetUserId();

        try
        {
            int totalPrice = this.repairmentService.CalculateTheCostOfRepairmanetAsync(model.Type, model.Quantity);
            model.Cost = totalPrice / model.Quantity;
            if (!await this.driverService.SpendForRepairment(mechanicId, totalPrice))
            {
                TempData[ErrorMessage] = $"Insufficient balance!";
                return View(model);
            }

            await this.repairmentService.RepairTrailerAsync(mechanicId, model);
            TempData[SuccessMessage] = $"You successfully repaired your trailer! Total price was: {totalPrice} EUR";
        }
        catch (Exception)
        {
            TempData[ErrorMessage] = "Something happened while repairing your trailer!";
            return View(model);
        }

        return RedirectToAction("MyRepairments", "Repairment");
    }

    public async Task<IActionResult> MyRepairments()
    {
        IEnumerable<AllRepairmentsViewModel> repairments = await this.repairmentService.GetAllRepairmentsByUserIdAsync(this.GetUserId());

        return View(repairments);
    }
}

