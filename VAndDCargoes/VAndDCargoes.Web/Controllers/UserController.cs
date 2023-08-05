using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VAndDCargoes.Data.Models;
using VAndDCargoes.Web.ViewModels.User;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authentication;

namespace VAndDCargoes.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMemoryCache memoryCache;

        public UserController(SignInManager<ApplicationUser> signInManager,
                              UserManager<ApplicationUser> userManager,
                              IMemoryCache memoryCache)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.memoryCache = memoryCache;
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

            IdentityResult result = await this.userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    this.ModelState.AddModelError(string.Empty, error.Description);
                }

                return View(model);
            }

            await this.signInManager.SignInAsync(user, false);
            this.memoryCache.Remove(memoryCache);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public async Task<IActionResult> Login(string? returnlUrl = null)
        {
            await this.HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = returnlUrl
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var result =
                await signInManager.PasswordSignInAsync(model.Username
                , model.Password, model.RememberMe, false);

            if (!result.Succeeded)
            {
                this.ModelState.AddModelError(string.Empty, "There was an error while logging you in! Please try again later or contact an administrator.");

                return View(model);
            }

            return Redirect(model.ReturnUrl ?? "/Home/Index");
        }
    }
}

