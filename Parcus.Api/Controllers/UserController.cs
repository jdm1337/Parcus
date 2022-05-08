using Microsoft.AspNetCore.Mvc;

namespace Parcus.Api.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
