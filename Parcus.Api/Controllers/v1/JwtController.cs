using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.DTO.Outgoing;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;
using Parcus.Domain.Settings;
using Parcus.Services.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Parcus.Web.Controllers.v1
{
    public class JwtController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ITokenService _tokenService;
        public JwtController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsMonitor<JwtSettings> optionsMonitor,
            ITokenService tokenService) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = optionsMonitor.CurrentValue;
            _tokenService = tokenService;
        }

         /// <summary>
         /// Обновление access и refresh токенов
         /// </summary>
         [AllowAnonymous]
         [HttpPost]
         [Route("refresh-token")]
         public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
         {

             var user = await _tokenService.GetUserFromTokenAsync(request.AccessToken);

             if (user == null)
                return BadRequest("Invalid access token or refresh token");
             
             if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
             {
                 return BadRequest("Invalid access token or refresh token");
             }

             var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
             var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

             await _userManager.UpdateRefreshTokenFieldsAsync(user, refreshToken, Convert.ToInt32(_jwtSettings.RefreshTokenValidityInDays));

             return new ObjectResult(new RefreshTokenResponse
             {
                 AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                 RefreshToken = refreshToken
             });
         }

         /// <summary>
         /// Отзыв access токена
         /// </summary>
         [Authorize(Permissions.Jwt.RevokeAccessToken)]
         [HttpPost]
         [Route("{id}/revoke")]
         public async Task<IActionResult> Revoke(string id)
         {
             var user = await _userManager.FindByIdAsync(id);

             if (user == null) 
                return BadRequest("Invalid userId");

             user.RefreshToken = null;
             await _userManager.UpdateAsync(user);

             return NoContent();
         }

         /// <summary>
         /// Отзыв access токенов
         /// </summary>
         [Authorize(Permissions.Jwt.RevokeAccessToken)]
         [HttpPost]
         [Route("revoke-all")]
         public async Task<IActionResult> RevokeAll()
         {
             var users = _userManager.Users.ToList();
             foreach (var user in users)
             {
                 user.RefreshToken = null;
                 await _userManager.UpdateAsync(user);
             }
             return NoContent();
         }
     }
 }

