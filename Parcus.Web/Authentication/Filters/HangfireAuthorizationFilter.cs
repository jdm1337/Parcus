using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Parcus.Web.Authentication.Filters
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private IServiceScopeFactory _serviceScopeFactory;
        private const string AllowedRole = "Administrators";
        public HangfireAuthorizationFilter(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var accessToken = context.GetHttpContext().Request.Cookies["access-token"];
            if(accessToken == null)
                return false;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
                var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

                var user = tokenService.GetUserFromTokenAsync(accessToken).Result;

                if(user == null)
                    return false;
                
                var userRoles = userManager.GetRolesAsync(user).Result;

                var isAdmin = userRoles.Where(x => x.Equals(AllowedRole)).Any();

                if (isAdmin)
                    return true;
                
                return false;
            }
        }
    }
}
