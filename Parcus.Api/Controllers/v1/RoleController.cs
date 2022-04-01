using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Claims;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;
using System.Security.Claims;

namespace Parcus.Api.Controllers.v1
{
    public class RoleController : BaseController
    {
        private readonly RoleManager<Role> _roleManager;
        public RoleController(
            IUnitOfWork unitOfWork,
            RoleManager<Role> roleManager
            ) : base(unitOfWork)
        {
            _roleManager = roleManager;
        }
        [Authorize(Permissions.Roles.Create)]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest createRoleRequest)
        {
            Role newRole = new Role
            {
                Name = createRoleRequest.RoleName,
                Description = createRoleRequest.Description
            };
            var created = await _roleManager.CreateAsync(newRole);
            if (created.Succeeded)
            {
                foreach (var permission in createRoleRequest.Permissions)
                {
                    await _roleManager.AddClaimAsync(newRole,
                        new Claim(CustomClaimTypes.Permission, permission));
                }
                return Ok();
            }
            return BadRequest();
        }
        [Authorize(Permissions.Roles.Delete)]
        [HttpDelete]
        [Route("Delete/{roleName}")]
        public async Task<IActionResult> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return BadRequest();
            }
            await _roleManager.DeleteAsync(role);
            return Ok();

        }
        [Authorize(Permissions.Roles.GetPermissions)]
        [HttpGet]
        [Route("Permissions/{roleName}")]
        public async Task<IActionResult> Permission(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var claims = await _roleManager.GetClaimsAsync(role);
            List<string> permissions = new List<string>();
            if (role == null)
            {
                return BadRequest();
            }
            foreach (var roleClaim in claims)
            {
                if (roleClaim.Type.Equals("Permissions"))
                {
                    permissions.Add(roleClaim.Value);
                }
            }
            return Ok( new GetPermissionsRequest
            {
                Permissions = permissions
            }) ;
        }
        [Authorize(Permissions.Roles.AddPermission)]
        [HttpPost]
        [Route("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest addPermissionRequest)
        {
            var role = await _roleManager.FindByNameAsync(addPermissionRequest.RoleName);
            if(role == null)
            {
                return BadRequest();
            }
            var permission = new Claim(CustomClaimTypes.Permission, addPermissionRequest.PermissionName);


            var result = await _roleManager.AddClaimAsync(role, permission);
            if (result.Succeeded)
            {
                return Ok();
            }
            return BadRequest();
        }

    }
}
