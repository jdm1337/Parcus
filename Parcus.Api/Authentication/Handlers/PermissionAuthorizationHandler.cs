using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Parcus.Api.Authentication.Claims;
using Parcus.Api.Authentication.Requirments;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain;

namespace Parcus.Api.Authentication.Handlers
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IAuthService _authService;
        


        public PermissionAuthorizationHandler(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IHttpContextAccessor httpContextAccessor,
            IAuthService authService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _contextAccessor = httpContextAccessor;
            _authService = authService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var request = _contextAccessor.HttpContext.Request;
            var auth = request.Headers.Authorization;
            string[] authTypeAndToken;
            try
            {
                authTypeAndToken = auth.ToString().Split(" ");
            }
            catch
            {
                return;
            }
           
            var token = authTypeAndToken[1];

            var user = await _authService.GetUserFromToken(token);

            
            if (user == null)
            {
                return;
            }
            
            // Get all the roles the user belongs to and check if any of the roles has the permission required
            // for the authorization to succeed.
              
            var userRoleNames = await _userManager.GetRolesAsync(user);
            var userRoles = _roleManager.Roles.Where(x => userRoleNames.Contains(x.Name));
            
            foreach (var role in userRoles)
            {
                var roleClaims = await _roleManager.GetClaimsAsync(role);
                var permissions = roleClaims.Where(x => x.Type == CustomClaimTypes.Permission &&
                                                        x.Value == requirement.Permission &&
                                                        x.Issuer == "LOCAL AUTHORITY")
                                            .Select(x => x.Value);

                if (permissions.Any())
                {
                    context.Succeed(requirement);
                    return;
                }
            }
        }
    }
}
