using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;

using System.Security.Claims;

using System.Security.Principal;


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
            
            var user = await _userManager.FindByIdAsync(userId);
            if(user == null)  
                return null; 

            return userId; 
        }
        
        public async Task<List<string>> GetUserPermissionsAsync(User user)
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
