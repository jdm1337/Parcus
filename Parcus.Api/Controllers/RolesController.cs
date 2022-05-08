using Microsoft.AspNetCore.Mvc;

namespace Parcus.Api.Controllers
{
    public class RolesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
