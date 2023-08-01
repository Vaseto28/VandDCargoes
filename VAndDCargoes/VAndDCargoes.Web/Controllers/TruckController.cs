using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Truck;

namespace VAndDCargoes.Web.Controllers;

public class TruckController : BaseController
{
    private readonly ITruckService truckService;

    public TruckController(ITruckService truckService)
    {
        this.truckService = truckService;
    }

    [HttpGet]
    public IActionResult Add()
    {
        TruckAddViewModel model = new TruckAddViewModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TruckAddViewModel model)
    {
        string userId = this.GetUserId();

        DateTime createdOn;

        if (!DateTime.TryParse(model.CreatedOn, out createdOn))
        {
            this.ModelState.AddModelError("Create truck", "Invalid date format! All dates must follow this format: dd/MM/yyyy");

            return View(model);
        }

        if (this.truckService.IsConditionValid(model.Condition))
        {
            this.ModelState.AddModelError("Create truck", "Invalid condition!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this.truckService.AddTruckAsync(model, userId);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Create truck", "Invalid data!");

            return View(model);
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
    public async Task<IActionResult> Edit(TruckEditViewModel model, string id)
    {
        DateTime createdOn;

        if (!DateTime.TryParse(model.CreatedOn, out createdOn))
        {
            this.ModelState.AddModelError("Edit truck", "Invalid date format! All dates must follow this format: dd/MM/yyyy");

            return View(model);
        }

        if (this.truckService.IsConditionValid(model.Condition))
        {
            this.ModelState.AddModelError("Edit truck", "Invalid condition!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this.truckService.EditTruckAsync(model, id);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Edit truck", "Invalid data!");

            return View(model);
        }

        return RedirectToAction("Details", "Truck");
    }

    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await this.truckService.DeletebyIdAsync(id);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Delete truck", "The operation wasn't successful!");

            return RedirectToAction("Details", "Truck");
        }

        return RedirectToAction("All", "Truck");
    }
}

