using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;


        public AuthService(
            IOptionsMonitor<JwtSettings> optionsMonitor,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _jwtSettings = optionsMonitor.CurrentValue;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<string> GetUserIdFromRequest(IIdentity userIdentity)
        {
            var claimsIdentity = userIdentity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst("id")?.Value;
            return userId;
        }

        public async Task<List<Claim>> GetUsersClaimsForTokenAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var authClaims = new List<Claim>
                {
                    new Claim("id", Convert.ToString(user.Id) ),
                    new Claim(ClaimTypes.Email, user.Email),

                };

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }
            return authClaims;
        }
        public async Task<List<string>> GetPermissionsFromUserAsync(User user)
        {
            List<string> permissions = new List<string>();
            var userRoleNames = await _userManager.GetRolesAsync(user);
            
            foreach(var roleName in userRoleNames)
            {
                var userRole = await _roleManager.FindByNameAsync(roleName);
                var roleClaims =  await _roleManager.GetClaimsAsync(userRole);
                if(roleClaims.Any())
                    foreach(var claim in roleClaims)
                    {
                        permissions.Add(claim.Value);
                    }
            }
            
            return permissions;
        }
    }
}
