using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Services.Contracts;
using VAndDCargoes.Web.ViewModels.Trailer;
using static VAndDCargoes.Common.NotificationMessagesConstants;

namespace VAndDCargoes.Web.Controllers;

public class TrailerController : BaseController
{
    private readonly ITrailerService trailerService;

    public TrailerController(ITrailerService trailerService)
    {
        this.trailerService = trailerService;
    }

    [Authorize(Roles = "Administrator, Specialist")]
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
            TempData[SuccessMessage] = "You've successfully created a trailer!";
        }
        catch (Exception)
        {
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
            TempData[SuccessMessage] = "You've successfully edited your trailer!";
        }
        catch (Exception)
        {
            return View(model);
        }

        return RedirectToAction("All", "Trailer");
    }

    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await this.trailerService.DeleteTrailerByIdAsync(id);
            TempData[SuccessMessage] = "You've successfully deleted your trailer!";
        }
        catch (Exception)
        {
            return RedirectToAction("Details", "Trailer");
        }

        return RedirectToAction("All", "Trailer");
    }

    public async Task<IActionResult> MyTrailers([FromQuery]TrailerQueryAllViewModel queryModel)
    {
        string userId = this.GetUserId();

        queryModel.Trailers = await this.trailerService.GetAllTrailersCreatedByUserByIdAsync(userId, queryModel);

        return View(queryModel);
    }

    public async Task<IActionResult> DrivenTrailers()
    {
        string userId = this.GetUserId();

        IEnumerable<TrailerAllViewModel> trailers = await this.trailerService.GetAllTrailersDrivenByUserByIdAsync(userId);

        return View(trailers);
    }

    public async Task<IActionResult> Drive(string id)
    {
        string userId = this.GetUserId();

        if (await this.trailerService.IsUserAlreadyDrivingTrailerByIdAsync(userId, id) &&
            await this.trailerService.DriveTrailerByIdASync(userId, id))
        {
            TempData[InformationMessage] = "You're now using this trailer!";
            return RedirectToAction("DrivenTrailers", "Trailer");
        }

        return RedirectToAction("All", "Trailer");
    }

    public async Task<IActionResult> Release(string id)
    {
        string userId = this.GetUserId();

        if (!await this.trailerService.IsUserAlreadyDrivingTrailerByIdAsync(userId, id) &&
            await this.trailerService.ReleaseTrailerByIdAsync(userId, id))
        {
            TempData[InformationMessage] = "You've successfully released the trailer!";
            return RedirectToAction("All", "Trailer");
        }

        return RedirectToAction("DrivenTrailers", "Trailer");
    }
}

