using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Cargo;
using static VAndDCargoes.Common.EntitiesValidations.Cargo;

namespace VAndDCargoes.Web.Controllers;

public class CargoController : BaseController
{
    private readonly ICargoService cargoService;

    public CargoController(ICargoService cargoService)
    {
        this.cargoService = cargoService;
    }

    [HttpGet]
    public IActionResult Add()
    {
        CargoAddViewModel model = new CargoAddViewModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(CargoAddViewModel model)
    {
        string userId = this.GetUserId();

        if (this.cargoService.IsCategoryValid(model.Category) ||
            this.cargoService.IsPhysicalStateValid(model.PhysicalState))
        {
            this.ModelState.AddModelError("Create cargo", "Invalid category or physical state!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this.cargoService.AddCargoAsync(model, userId);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Add cargo", "Invalid data!");

            return View(model);
        }

        return RedirectToAction("All", "Cargo");
    }

    public async Task<IActionResult> All([FromQuery] CargoQueryAllViewModel queryModel)
    {
        queryModel.Cargoes = await this.cargoService.GetAllCargoesAsync(queryModel);

        return View(queryModel);
    }

    public async Task<IActionResult> Details(string id)
    {
        CargoDetailsViewModel? cargo = await this.cargoService.GetCargoDetailsById(id);

        if (cargo == null)
        {
            this.ModelState.AddModelError("Cargo's details", "There is no such a cargo!");
        }

        if (!this.ModelState.IsValid)
        {
            return RedirectToAction("All", "Cargo");
        }

        return View(cargo);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        CargoEditViewModel? cargo = await this.cargoService.GetCargoForEditByIdAsync(id);

        if (cargo == null)
        {
            this.ModelState.AddModelError("Edit cargo", "There is no such a cargo!");
        }

        if (!this.ModelState.IsValid)
        {
            return RedirectToAction("All", "Cargo");
        }

        return View(cargo);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(CargoEditViewModel model, string id)
    {
        if (this.cargoService.IsCategoryValid(model.Category) ||
            this.cargoService.IsPhysicalStateValid(model.PhysicalState))
        {
            this.ModelState.AddModelError("Edit cargo", "Invalid category or physical state!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this.cargoService.EditCargoAsync(model, id);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError(nameof(model), "Invalid data!");
            return View(model);
        }

        return RedirectToAction("Details", "Cargo");
    }

    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await this.cargoService.DeleteCargoByIdAsync(id);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Delete cargo", "The operation wasn't successful!");

            return RedirectToAction("Details", "Cargo");
        }

        return RedirectToAction("All", "Cargo");
    }
}

