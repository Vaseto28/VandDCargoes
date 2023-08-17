using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Repairment;
using static VAndDCargoes.Common.NotificationMessagesConstants;

namespace VAndDCargoes.Web.Controllers;

public class RepairmentController : BaseController
{
    private readonly ITruckService truckService;
    private readonly IRepairmentService repairmentService;
    private readonly IDriverService driverService;

    public RepairmentController(ITruckService truckService, IRepairmentService repairmentService, IDriverService driverService)
    {
        this.truckService = truckService;
        this.repairmentService = repairmentService;
        this.driverService = driverService;
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
            if (!await this.driverService.SpendForRepairment(mechanicId, this.repairmentService.CalculateTheCostOfRepairmanetAsync(model)))
            {
                TempData[ErrorMessage] = $"Insufficient balance!";
                return View(model);
            }
            
            await this.repairmentService.RepairTruckAsync(mechanicId, model);
            TempData[SuccessMessage] = $"You successfully repaired your truck! Total price was: {model.Cost * model.Quantity} EUR";
        }
        catch (Exception)
        {
            TempData[ErrorMessage] = "Something happened while repairing your truck!";
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

