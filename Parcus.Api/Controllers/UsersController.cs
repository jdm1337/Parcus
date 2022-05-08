using Microsoft.AspNetCore.Mvc;

namespace Parcus.Api.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
