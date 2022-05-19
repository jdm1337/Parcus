using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Claims;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.Identity;
using Parcus.Domain.Pagination;
using Parcus.Domain.Permission;
using System.Security.Claims;

namespace Parcus.Web.Controllers.v1
{
    public class RolesController : BaseController
    {
        private readonly RoleManager<Role> _roleManager;
        public RolesController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            RoleManager<Role> roleManager
            ) : base(unitOfWork, mapper)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Создание роли
        /// </summary>
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
                    await _roleManager.AddClaimAsync(newRole, new Claim(CustomClaimTypes.Permission, permission));
                }
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// Получение ролей
        /// </summary>
        [Authorize(Permissions.Roles.GetRoles)]
        [HttpGet]
        [Route("Select")]
        public async Task<IActionResult> Permission([FromQuery]RoleParameters parameters)
        {
            var users = await _unitOfWork.Roles.GetRoles(parameters);
            var responseMetadata = new
            {
                users.TotalCount,
                users.PageSize,
                users.CurrentPage,
                users.TotalPages,
                users.HasNext,
                users.HasPrevious
            };
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(responseMetadata));

            return Ok(users);
        }

        /// <summary>
        /// Удаление роли
        /// </summary>
        [Authorize(Permissions.Roles.Delete)]
        [HttpDelete]
        [Route("{roleName}/Delete")]
        public async Task<IActionResult> Delete(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null) 
                return BadRequest();

            var deleteResult = await _roleManager.DeleteAsync(role);

            if (!deleteResult.Succeeded) 
                return BadRequest();

            return Ok();
        }
        /// <summary>
        /// Получение разрешений роли
        /// </summary>
        [Authorize(Permissions.Roles.GetPermissions)]
        [HttpGet]
        [Route("{roleName}/Permissions")]
        public async Task<IActionResult> Permission(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null) 
                return BadRequest();

            var claims = await _roleManager.GetClaimsAsync(role);
            List<string> permissions = new List<string>();
            
            foreach (var roleClaim in claims)
            {
                if (roleClaim.Type.Equals("Permissions"))
                {
                    permissions.Add(roleClaim.Value);
                }
            }
            return Ok(permissions);
        }

        /// <summary>
        /// Добавление разрешения к роли
        /// </summary>
        [Authorize(Permissions.Roles.AddPermission)]
        [HttpPost]
        [Route("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] AddPermissionRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if(role == null)
                return BadRequest();
            
            var permissionExist = (await _roleManager.GetClaimsAsync(role))
                .Where(p => p.Type == CustomClaimTypes.Permission
                       &&
                       p.Value == request.PermissionName)
                .Any();

            if (permissionExist)  
                return BadRequest("Permission already in role.");

            var permission = new Claim(CustomClaimTypes.Permission, request.PermissionName);
            var result = await _roleManager.AddClaimAsync(role, permission);

            if (!result.Succeeded) 
                return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Удаление разрешение у роли
        /// </summary>
        [Authorize(Permissions.Roles.DeletePermission)]
        [HttpDelete]
        [Route("DeletePermission")]
        public async Task<IActionResult> DeletePermission([FromBody] DeletePermissionRequest request)
        {
            var role = await _roleManager.FindByNameAsync(request.RoleName);

            if (role == null) 
                return BadRequest();

            var permission = (await _roleManager.GetClaimsAsync(role))
                .Where(p => p.Type == CustomClaimTypes.Permission
                       &&
                       p.Value == request.PermissionName)
                .FirstOrDefault();
                
            if (permission ==null) 
                return BadRequest("Permission not exist.");

            await _roleManager.RemoveClaimAsync(role, permission);
            return Ok();
        }
    }
}
