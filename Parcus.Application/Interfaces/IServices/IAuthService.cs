using Parcus.Domain.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<List<Claim>> GetUsersClaimsForTokenAsync(User user);
        
        Task<string> GetUserIdFromRequest(IIdentity? identity);
        Task<List<string>> GetPermissionsFromUserAsync(User user);
    }
}
