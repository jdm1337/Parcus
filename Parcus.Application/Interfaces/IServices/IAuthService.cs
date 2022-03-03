﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateTokenAsync(List<Claim> authClaims);
        Task<string> GenerateRefreshTokenAsync();
        Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string? token);


    }
}