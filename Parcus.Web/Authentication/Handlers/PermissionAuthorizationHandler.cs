using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Parcus.Domain.Claims;
using Parcus.Web.Authentication.Requirments;

using Parcus.Domain.Identity;

namespace Parcus.Web.Authentication.Handlers
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        
        public PermissionAuthorizationHandler(
            UserManager<User> userManager,
            RoleManager<Role> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userIdClaim = context.User.Claims.Where(u => u.Type.Equals("id")).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userIdClaim?.Value);
            
            if (user == null)
                return;
           
            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.  
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name));

                foreach (var role in userRoles)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    var permissions = roleClaims
                        .Where(x => x.Type == CustomClaimTypes.Permission &&
                                    x.Value == requirement.Permission &&
                                    x.Issuer == "LOCAL AUTHORITY");

                    if (permissions.Any())
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }
        }
    }
}
