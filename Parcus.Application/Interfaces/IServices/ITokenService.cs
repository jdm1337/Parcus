using Parcus.Domain.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Parcus.Application.Interfaces.IServices
{
    public interface ITokenService
    {
        Task<JwtSecurityToken> GenerateAccessTokenAsync(User user);
        Task<string> GenerateRefreshTokenAsync();
        Task<ClaimsPrincipal?> GetPrincipalFromExpiredTokenAsync(string? token);
        Task<User> GetUserFromTokenAsync(string? token);
    }
}
