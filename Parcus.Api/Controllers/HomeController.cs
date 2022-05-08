using Microsoft.AspNetCore.Mvc;

namespace Parcus.Api.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
