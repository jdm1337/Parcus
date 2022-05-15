using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Parcus.Services.Services
{
     public class TokenService : ITokenService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        public TokenService(
            IOptionsMonitor<JwtSettings> optionsMonitor,
            UserManager<User> userManager)
        {
            _jwtSettings = optionsMonitor.CurrentValue;
            _userManager = userManager;
        }
        public async Task<JwtSecurityToken> GenerateAccessTokenAsync(User user)
        {
            var userClaims = await GetUserClaimsAsync(user);

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            var accessTokenValidityInMinutes = Convert.ToInt32(_jwtSettings.AccessTokenValidityInMinutes);

            var jwtSecurityToken = new JwtSecurityToken(
               issuer: _jwtSettings.ValidIssuer,
               audience: _jwtSettings.ValidAudience,
               expires: DateTime.Now.AddMinutes(accessTokenValidityInMinutes),
               claims: userClaims,

               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );

            return jwtSecurityToken;

        }
        
        public async Task<string> GenerateRefreshTokenAsync()
        {
            Random rand = new Random();

            int randValue;
            StringBuilder refreshToken = new StringBuilder();
            char letter;
            for (int i = 0; i < 64; i++)
            {     
                randValue = rand.Next(0, 26);
                letter = Convert.ToChar(randValue + 65);
                refreshToken.Append(letter);
            }
            return refreshToken.ToString();
            
        }
        public async Task<ClaimsPrincipal?> GetPrincipalFromExpiredTokenAsync(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<User> GetUserFromTokenAsync(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

                if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                if (emailClaim is null)
                {
                    throw new SecurityTokenException("Invalid token");
                }

                return await _userManager.FindByEmailAsync(emailClaim.Value);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
        private async Task<List<Claim>> GetUserClaimsAsync(User user)
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
    }
}
