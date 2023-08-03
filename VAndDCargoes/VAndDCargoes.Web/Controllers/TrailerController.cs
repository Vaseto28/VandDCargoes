using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Trailer;

namespace VAndDCargoes.Web.Controllers;

public class TrailerController : BaseController
{
    private readonly ITrailerService trailerService;

    public TrailerController(ITrailerService trailerService)
    {
        this.trailerService = trailerService;
    }

    //[Authorize(Roles = "Administrator")]
    [HttpGet]
    public IActionResult Add()
    {
        TrailerAddViewModel model = new TrailerAddViewModel();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(TrailerAddViewModel model)
    {
        string userId = this.GetUserId();

        if (this.trailerService.IsCategoryValid(model.Category) ||
            this.trailerService.IsConditionValid(model.Condition) ||
            this.trailerService.IsDementionsValid(model.Dementions))
        {
            this.ModelState.AddModelError("Create trailer", "Invalid category, condition or dementions!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this.trailerService.AddTrailerAsync(model, userId);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Add trailer", "Invalid data!");

            return View(model);
        }

        return RedirectToAction("All", "Trailer");
    }

    public async Task<IActionResult> All([FromQuery]TrailerQueryAllViewModel queryModel)
    {
        queryModel.Trailers = await this.trailerService.GetAllTrailersAsync(queryModel);

        return View(queryModel);
    }

    public async Task<IActionResult> Details(string id)
    {
        TrailerDetailsViewModel? model = await this.trailerService.GetTrailerDetailsByIdAsync(id);

        if (model == null)
        {
            return RedirectToAction("All", "Trailer");
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(string id)
    {
        TrailerEditViewModel? model = await this.trailerService.GetTrailerForEditByIdAsync(id);

        if (model == null)
        {
            return RedirectToAction("All", "Trailer");
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(TrailerEditViewModel model, string id)
    {
        if (this.trailerService.IsCategoryValid(model.Category) ||
            this.trailerService.IsConditionValid(model.Condition) ||
            this.trailerService.IsDementionsValid(model.Dementions))
        {
            this.ModelState.AddModelError("Edit trailer", "Invalid category, condition or dementions!");
        }

        if (!this.ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            await this.trailerService.EditTrailerAsync(model, id);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Edit trailer", "Invalid data!");

            return View(model);
        }

        return RedirectToAction("All", "Trailer");
    }

    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await this.trailerService.DeleteTrailerByIdAsync(id);
        }
        catch (Exception)
        {
            this.ModelState.AddModelError("Delete trailer", "The operaation wasn't succesful!");

            return RedirectToAction("Details", "Trailer");
        }

        return RedirectToAction("All", "Trailer");
    }
}

