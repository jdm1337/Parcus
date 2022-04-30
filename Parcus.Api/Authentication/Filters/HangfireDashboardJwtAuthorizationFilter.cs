using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Parcus.Api.Authentication.Filters
{
    public class HangfireDashboardJwtAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private TokenValidationParameters tokenValidationParameters;
        private string role;
        public HangfireDashboardJwtAuthorizationFilter(TokenValidationParameters tokenValidationParameters, string role = null)
        {
               this.tokenValidationParameters = tokenValidationParameters;
               this.role = role;
        }
        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var authorizationHeader = httpContext.Request.Headers.Authorization;
            
            if(authorizationHeader.IsNullOrEmpty())
            {
                return false;
            }
            var tokenWithBearerPrefix = authorizationHeader.ToString().Split(' ');

            if(tokenWithBearerPrefix.Length != 2)
            {
                return false;
            }

            var accessToken = tokenWithBearerPrefix[1];
            try
            {
                SecurityToken validatedToken = null;
                JwtSecurityTokenHandler hand = new JwtSecurityTokenHandler();
                var claims = hand.ValidateToken(accessToken, this.tokenValidationParameters, out validatedToken);

                if (!String.IsNullOrEmpty(this.role) && !claims.IsInRole(this.role))
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
