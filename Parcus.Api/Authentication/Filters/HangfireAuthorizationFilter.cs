using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Parcus.Api.Authentication.Filters
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
            var httpContext = context.GetHttpContext();
            var authorizationHeader = httpContext.Request.Headers.Authorization;

            if (authorizationHeader.IsNullOrEmpty())
                return false;
            
            var tokenWithBearerPrefix = authorizationHeader.ToString().Split(' ');

            if (tokenWithBearerPrefix.Length != 2)
                return false;
            
            var accessToken = tokenWithBearerPrefix[1];

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
