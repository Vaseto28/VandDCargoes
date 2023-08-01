using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Web.ViewModels.Driver;
using VAndDCargoes.Services.Contracts;

namespace VAndDCargoes.Web.Controllers
{
	public class DriverController : BaseController
	{
		private readonly IDriverService driverService;

		public DriverController(IDriverService driverService)
		{
			this.driverService = driverService;
		}

		[HttpGet]
		public async Task<IActionResult> Become()
		{
			string id = this.GetUserId();

			if (await this.driverService.IsTheUserAlreadyDriver(id))
			{
				return RedirectToAction("All", "Truck");
			}

			BecomeDriverViewModel model = new BecomeDriverViewModel();

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Become(BecomeDriverViewModel model)
		{
			string userId = this.GetUserId();

			try
			{
				await this.driverService.BecomeDriverAsync(model, userId);
			}
			catch (Exception)
			{
				return View(model);
			}

			return RedirectToAction("All", "Truck");
		}
	}
}

