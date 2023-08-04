using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Web.ViewModels.User;

namespace VAndDCargoes.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUserStore<ApplicationUser> userStore;

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IUserStore<ApplicationUser> userStore)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.userStore = userStore;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            ApplicationUser user = new ApplicationUser();

            await this.userManager.SetUserNameAsync(user, model.Username);
            await this.userManager.SetEmailAsync(user, model.Email);

            IdentityResult result = await this.userManager.CreateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError("Register error", error.Description);
                }
            }

            await this.signInManager.SignInAsync(user, false);

            return RedirectToAction("Index", "Home");
        }
    }
}

