using Microsoft.AspNetCore.Mvc;

namespace Parcus.Api.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
