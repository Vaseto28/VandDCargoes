using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Repairment;

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
        string mechanicId = this.GetUserId();

        if (!await this.repairmentService.IsTruckConditionValidAsync(model.TruckId.ToString()) || !this.ModelState.IsValid)
        {
            TempData["ErrorMessage"] = "This truck is in perfect condition!";

            return View(model);
        }

        try
        {
            await this.repairmentService.RepairTruckAsync(mechanicId, model);
            await this.driverService.SpendForRepairment(mechanicId, model.Cost);
            TempData["SuccessMessage"] = "You successfully repaired your truck!";
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Something happened while repairing your truck!";
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

