using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Parcus.Api.Models.DTO.Outgoing;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.DTO.Outgoing;
using Parcus.Domain.Identity;
using Parcus.Domain.Pagination;
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
            IMapper mapper,
            RoleManager<Role> roleManager,
            IAuthService authService) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _authService = authService;
        }

        /// <summary>
        /// Получение разрешений пользователя по id
        /// </summary>
        [Authorize(Permissions.Users.GetPermissions)]
        [HttpGet]
        [Route("{id}/Permissions")]
        public async Task<IActionResult> Permission(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if(user == null) 
                return NotFound();

            return Ok(new GetPermissionsFromUserResponse
            {
                Permissions = await _authService.GetPermissionsFromUserAsync(user)
            });
        }

        /// <summary>
        /// Добавление пользователя к роли
        /// </summary>
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

        /// <summary>
        /// Получение пользователей
        /// </summary>
        [Authorize(Permissions.Users.GetUsers)]
        [HttpGet]
        [Route("Select")]
        public async Task<IActionResult> GetUsers([FromQuery]UserParameters parameters)
        {
            var users = await _unitOfWork.Users.GetUsers(parameters);
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
        /// Удаление пользователя
        /// </summary>
        [Authorize(Permissions.Users.Delete)]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return BadRequest();

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded) 
                return BadRequest();

            return Ok();
        }

        /// <summary>
        /// Обновление информации о пользователе
        /// </summary>
        [Authorize(Permissions.Users.Update)]
        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> Update(UpdateUserRequest request)
        {
            return Ok();
        }
    }
}
