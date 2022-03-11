﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
        public async Task<JwtSecurityToken> CreateTokenAsync(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
            _ = int.TryParse(_jwtSettings.TokenValidityInMinutes, out int tokenValidityInMinutes);
            var token = new JwtSecurityToken(
               issuer: _jwtSettings.ValidIssuer,
               audience: _jwtSettings.ValidAudience,
               expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
               claims: authClaims,
               
               signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
               );

            return token;
        }

        public async Task<string> GenerateRefreshTokenAsync()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        public async Task<ClaimsPrincipal?> GetPrincipalFromExpiredToken(string? token)
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
        public async Task<User> GetUserFromToken(string? token)
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
    }
}