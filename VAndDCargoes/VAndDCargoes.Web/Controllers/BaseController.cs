using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VAndDCargoes.Web.Controllers;

[Authorize]
public class BaseController : Controller
{
    protected string GetUserId()
    {
        return this.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}

