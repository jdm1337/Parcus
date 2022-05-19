using Microsoft.AspNetCore.Mvc;

namespace Parcus.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }

}
