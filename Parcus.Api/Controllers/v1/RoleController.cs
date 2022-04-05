using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Claims;
using Parcus.Domain.DTO.Incoming;
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
            IMapper mapper,
            RoleManager<Role> roleManager
            ) : base(unitOfWork, mapper)
        {
            _roleManager = roleManager;
        }

        [Authorize(Permissions.Roles.Create)]
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create([FromBody] CreateRoleRequest request)
        {
            Role newRole = new Role
            {
                Name = request.RoleName,
                Description = request.Description
            };
            var created = await _roleManager.CreateAsync(newRole);
            if (created.Succeeded)
            {
                foreach (var permission in request.Permissions)
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

            if (role == null) return BadRequest();
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
            return Ok(permissions);
        }

        [Authorize(Permissions.Roles.AddPermission)]
        [HttpPost]
        [Route("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);
            if(role == null)
            {
                return BadRequest();
            }

            var permissionExist = (await _roleManager.GetClaimsAsync(role))
                .Where(p => p.Type == CustomClaimTypes.Permission
                       &&
                       p.Value == request.PermissionName)
                .Any();

            if (permissionExist)  return BadRequest("Permission already in role.");

            var permission = new Claim(CustomClaimTypes.Permission, request.PermissionName);
            var result = await _roleManager.AddClaimAsync(role, permission);

            if (result.Succeeded) return Ok();
            
            return BadRequest();
        }

        [Authorize(Permissions.Roles.DeletePermission)]
        [HttpDelete]
        [Route("DeletePermission")]
        public async Task<IActionResult> DeletePermission([FromBody] DeletePermissionRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role == null) return BadRequest();

            var permission = (await _roleManager.GetClaimsAsync(role))
                .Where(p => p.Type == CustomClaimTypes.Permission
                       &&
                       p.Value == request.PermissionName).FirstOrDefault();
                
            if (permission ==null) return BadRequest("Permission not exist.");

            await _roleManager.RemoveClaimAsync(role, permission);
            return Ok();

        }
    }
}
