using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Cargo;

namespace VAndDCargoes.Web.Controllers;

public class CargoController : BaseController
{
    private readonly ICargoService cargoService;

    public CargoController(ICargoService cargoService)
    {
        this.cargoService = cargoService;
    }

    [Authorize(Roles = "Administrator, Specialist")]
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

        return RedirectToAction("All", "Cargo");
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

    public async Task<IActionResult> MyCargoes([FromQuery]CargoQueryAllViewModel queryModel)
    {
        string userId = this.GetUserId();

        queryModel.Cargoes = await this.cargoService.GetAllCargoesCreatedByUserByIdAsync(queryModel, userId);

        return View(queryModel);
    }

    public async Task<IActionResult> DeliveringCargoes()
    {
        string userId = this.GetUserId();

        IEnumerable<CargoAllViewModel> cargoes = await this.cargoService.GetAllCargoesDeliveringByUserByIdAsync(userId);

        return View(cargoes);
    }

    public async Task<IActionResult> Deliver(string id)
    {
        string userId = this.GetUserId();

        if (await this.cargoService.IsCargoStillDelivering(userId, id) &&
            await this.cargoService.DeliverCargoByIdAsync(userId, id))
        {
            return RedirectToAction("DeliveringCargoes", "Cargo");
        }

        return RedirectToAction("All", "Cargoes");
    }

    public async Task<IActionResult> Finish(string id)
    {
        string userId = this.GetUserId();

        if (!await this.cargoService.IsCargoStillDelivering(userId, id) &&
            await this.cargoService.FinishDeliveringOfCargoByIdAsync(userId, id))
        {
            return RedirectToAction("All", "Cargo");
        }

        return RedirectToAction("DeliveringCargoes", "Cargo");
    }
}

