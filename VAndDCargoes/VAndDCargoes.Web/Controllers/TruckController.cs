using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Truck;
using static VAndDCargoes.Common.NotificationMessagesConstants;

namespace VAndDCargoes.Web.Controllers;

public class TruckController : BaseController
{
    private readonly ITruckService truckService;

    public TruckController(ITruckService truckService)
    {
        this.truckService = truckService;
    }

    [Authorize(Roles = "Administrator, Specialist")]
    [HttpGet]
    public IActionResult Add()
    {
        TruckAddViewModel model = new TruckAddViewModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TruckAddViewModel addViewModel)
    {
        string userId = this.GetUserId();

        DateTime createdOn;

        if (!DateTime.TryParse(addViewModel.CreatedOn, out createdOn))
        {
            this.ModelState.AddModelError("Create truck", "Invalid date format! All dates must follow this format: MM/dd/yyyy");

            return View(addViewModel);
        }

        if (this.truckService.IsConditionValid(addViewModel.Condition))
        {
            this.ModelState.AddModelError("Create truck", "Invalid condition!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(addViewModel);
        }

        try
        {
            await this.truckService.AddTruckAsync(addViewModel, userId);
            TempData[SuccessMessage] = "You've successfully created a truck!";
        }
        catch (Exception)
        {
            return View(addViewModel);
        }

        return RedirectToAction("All", "Truck");
    }

    public async Task<IActionResult> All([FromQuery]TruckQueryAllModel queryModel)
    {
        queryModel.Trucks = await this.truckService.GetAllTrucksAsync(queryModel);

        return View(queryModel);
    }

    public async Task<IActionResult> Details(string id)
    {
        TruckDetailsViewModel? truckDetails = await this.truckService.GetTruckDetailsByIdAsync(id);

        if (truckDetails == null)
        {
            return RedirectToAction("All", "Truck");
        }

        return View(truckDetails);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        TruckEditViewModel? truckEdit = await this.truckService.GetTruckByIdForEditAsync(id);

        if (truckEdit == null)
        {
            return RedirectToAction("All", "Truck");
        }

        return View(truckEdit);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TruckEditViewModel editViewModel, string id)
    {
        DateTime createdOn;

        if (!DateTime.TryParse(editViewModel.CreatedOn, out createdOn))
        {
            this.ModelState.AddModelError("Edit truck", "Invalid date format! All dates must follow this format: dd/MM/yyyy");

            return View(editViewModel);
        }

        if (this.truckService.IsConditionValid(editViewModel.Condition))
        {
            this.ModelState.AddModelError("Edit truck", "Invalid condition!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(editViewModel);
        }

        try
        {
            await this.truckService.EditTruckAsync(editViewModel, id);
            TempData[SuccessMessage] = "You've successfully edited your truck!";
        }
        catch (Exception)
        {
            return View(editViewModel);
        }

        return RedirectToAction("All", "Truck");
    }

    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await this.truckService.DeletebyIdAsync(id);
            TempData[SuccessMessage] = "You've successfully deleted your truck!";
        }
        catch (Exception)
        {
            return RedirectToAction("Details", "Truck");
        }

        return RedirectToAction("All", "Truck");
    }

    public async Task<IActionResult> MyTrucks([FromQuery]TruckQueryAllModel queryModel)
    {
        string userId = this.GetUserId();

        queryModel.Trucks = await this.truckService.GetAllTrucksCreatedByUserByIdAsync(userId, queryModel);

        return View(queryModel);
    }

    public async Task<IActionResult> DrivenTrucks()
    {
        string userId = this.GetUserId();

        IEnumerable<TruckAllViewModel> model = await this.truckService.GetAllTrucksDrivenByUserByIdAsync(userId);

        return View(model);
    }

    public async Task<IActionResult> Drive(string id)
    {
        string userId = this.GetUserId();

        if (await this.truckService.IsUserAlreadyDrivingTruckByIdAsync(userId, id) &&
            await this.truckService.DriveTruckByIdAsync(userId, id))
        {
            TempData[InformationMessage] = "You're now driving a truck!";
            return RedirectToAction("DrivenTrucks", "Truck");
        }

        return RedirectToAction("All", "Truck");
    }

    public async Task<IActionResult> Release(string id)
    {
        string userId = this.GetUserId();

        if (!await this.truckService.IsUserAlreadyDrivingTruckByIdAsync(userId, id) &&
            await this.truckService.ReleaseTruckByIdAsync(userId, id))
        {
            TempData[InformationMessage] = "You've successfully released the truck!";
            return RedirectToAction("All", "Truck");
        }

        return RedirectToAction("DrivenTrucks", "Truck");
    }
}

