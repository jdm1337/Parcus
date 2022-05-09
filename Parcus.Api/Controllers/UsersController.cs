using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
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
        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
    }
}
