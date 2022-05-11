using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Parcus.Persistence.Data;
using Parcus.Api.Models;

namespace Parcus.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme, Roles ="Administrators")]
    public class HomeController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public IActionResult Index()
        {
            var usersCount = _appDbContext.Users.Count();
            var rolesCount = _appDbContext.Roles.Count();
            var instrumentsCount = _appDbContext.Instruments.Count();
            var portfoliosCount = _appDbContext.BrokeragePortfolios.Count();
            return View(new HomeViewModel
            {
                RegisteredUser = usersCount,
                RolesAmount = rolesCount,
                InstrumentsAmount = instrumentsCount,
                PortfoliosAmount = portfoliosCount,
            });
        }
    }
}
