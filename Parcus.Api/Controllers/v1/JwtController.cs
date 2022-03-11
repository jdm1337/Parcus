﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Api.Models.DTO.Outgoing;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;
using Parcus.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Parcus.Api.Controllers.v1
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
            IOptionsMonitor<JwtSettings> optionsMonitor,
            ITokenService tokenService) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = optionsMonitor.CurrentValue;
            _tokenService = tokenService;
        }

            [HttpPost]
            [Route("refresh-token")]
            public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }
                string? accessToken = refreshTokenRequest.AccessToken;
                string? refreshToken = refreshTokenRequest.RefreshToken;

                var principal = await _tokenService.GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return BadRequest("Invalid access token or refresh token");
                }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                var user = await _userManager.FindByEmailAsync(emailClaim.Value);

                if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Invalid access token or refresh token");
                }

                var newAccessToken = await _tokenService.CreateTokenAsync(principal.Claims.ToList());
                var newRefreshToken = await _tokenService.GenerateRefreshTokenAsync();

                user.RefreshToken = newRefreshToken;
                await _userManager.UpdateAsync(user);
                return new ObjectResult(new RefreshTokenResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                    RefreshToken = newRefreshToken
                });
            }
            [Authorize(Permissions.Jwt.RevokeAccessToken)]
            [HttpPost]
            [Route("revoke/{id}")]
            public async Task<IActionResult> Revoke(string id)
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null) return BadRequest("Invalid userId");

                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);

                return NoContent();
            }
            [Authorize]
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
