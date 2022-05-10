using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Identity;
using Parcus.Domain.Pagination;
using Parcus.Domain.Permission;
using Parcus.Persistence.Data;

namespace Parcus.Api.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class UsersController : Controller
    {
        protected IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly AppDbContext _appDbContext;
        public UsersController(
            IUnitOfWork unitOfWork,
            AppDbContext appDbContext,
            UserManager<User> userManager,
            RoleManager<Role> roleManager
            )
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index([FromQuery] UserParameters parameters)
        {
            var users = await _unitOfWork.Users.GetUsers(parameters);

            PageViewModel pageViewModel = new PageViewModel(users.TotalCount, users.CurrentPage, users.PageSize);
            UsersViewModel viewModel = new UsersViewModel
            {
                PageViewModel = pageViewModel,
                Users = users
            };
            return View(viewModel);
        }
        
        [HttpGet]
        public async Task<IActionResult> Profile(int id)
        {
            var user = await _userManager.FindByIdAsync("1");
            if (user == null)
                return NotFound();

            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            var role = await _roleManager.FindByNameAsync(roleName);

            var claims = await _roleManager.GetClaimsAsync(role);

            List<string> permissions = new List<string>();

            foreach (var roleClaim in claims)
            {
                if (roleClaim.Type.Equals("permission"))
                {
                    permissions.Add(roleClaim.Value);
                }
            }
            return View(new UserViewModel
            {
                Id = user.Id,
                Name = user.UserName,
                Email = user.Email,
                Role = roleName,
                Permissions = permissions
            });
        }
        
    }
}
