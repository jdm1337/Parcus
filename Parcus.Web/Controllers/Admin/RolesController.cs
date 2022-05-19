using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Parcus.Web.Models;
using Parcus.Domain.Identity;

namespace Parcus.Web.Controllers.Admin
{
    [Authorize(Roles = "Administrators,DemoUser")]
    public class RolesController : BaseAdminController
    {
        private readonly RoleManager<Role> _roleManager;
        public RolesController(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();

            return View(new RolesViewModel
            {
                Roles = roles
            });
        }
    }
}
