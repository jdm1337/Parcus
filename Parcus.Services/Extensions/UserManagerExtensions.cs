using Microsoft.AspNetCore.Identity;
using Parcus.Domain.Identity;


namespace Parcus.Services.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task UpdateRefreshTokenFieldsAsync(this UserManager<User> userManager, User user, string refreshToken, int validityInMinutes)
        {
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(validityInMinutes);
            await userManager.UpdateAsync(user);
        }
    }
}
