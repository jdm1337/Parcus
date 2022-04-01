using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Api.Models.DTO.Outgoing;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;

namespace Parcus.Api.Controllers.v1
{
    public class UsersController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IAuthService _authService;
        public UsersController(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IAuthService authService) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        [Authorize(Permissions.Users.GetPermissions)]
        [HttpGet]
        [Route("Permissions/{id}")]
        public async Task<IActionResult> Permission(string id)
        {
            var user = await _userManager.FindByIdAsync(id);


            return Ok(new GetPermissionsFromUserResponse
            {
                Permissions = await _authService.GetPermissionsFromUserAsync(user)
            });
        }

        [Authorize(Permissions.Users.AddToRole)]
        [HttpPost]
        [Route("AddToRole")]
        public async Task<IActionResult> AddToRole([FromBody] AddToRoleRequest addToRoleRequest)
        {
            var user = await _userManager.FindByIdAsync(addToRoleRequest.UserId);
            var role = await _roleManager.FindByIdAsync(addToRoleRequest.RoleId);

            if (user == null || role == null)
            {
                return BadRequest();
            }

            await _userManager.AddToRoleAsync(user, role.Name);
            return Ok();

        }

        [Authorize(Permissions.Users.GetUsers)]
        [HttpGet]
        [Route("Select")]
        public async Task<IActionResult> Select()
        {
            return Ok(new UserSelectResponse
            {
                Users = _userManager.Users
            });
            
        }
        [Authorize(Permissions.Users.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok();
        }
        [Authorize(Permissions.Users.Update)]
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id)
        {
            return Ok();
        }
    }
}
