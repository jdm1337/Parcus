using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Parcus.Persistence.Data;
using Parcus.Web.Models;

namespace Parcus.Web.Controllers.Admin
{
    [Authorize(Roles ="Administrators,DemoUser")]
    [Route("[controller]/[action]")]
    public class AdminController : BaseAdminController
    {
        private readonly AppDbContext _appDbContext;
        public AdminController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        [HttpGet]
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
