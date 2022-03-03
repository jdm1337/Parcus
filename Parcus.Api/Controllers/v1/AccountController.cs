using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Api.Models.DTO.Outgoing;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain;
using Parcus.Domain.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Parcus.Api.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        public AccountController(UserManager<User> userManager, RoleManager<Role> roleManager, IUnitOfWork unitOfWork, IOptionsMonitor<JwtSettings> optionsMonitor, IAuthService authService) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = optionsMonitor.CurrentValue;
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(new Response
                {
                    Status = "Fail",
                    Message = "Invalid statement"
                });
            }

            var existingUser = await _userManager.FindByEmailAsync(registrationRequest.Email);
            if (existingUser != null)
            {
                return BadRequest();
            }
            var newUser = new User
            {
                Email = registrationRequest.Email,
                UserName = registrationRequest.Email,
                EmailConfirmed = true, // Todo build fuctionallity to send email
            };
            var isCreated = await _userManager.CreateAsync(newUser, registrationRequest.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
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
                var token = await _authService.CreateTokenAsync(authClaims);
                var refreshToken = await _authService.GenerateRefreshTokenAsync();
                _ = int.TryParse(_jwtSettings.RefreshTokenValidityInDays, out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);
                
                await _userManager.UpdateAsync(user);
                return Ok(new LoginResponse
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }
            return BadRequest();
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest refreshTokenRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            string? accessToken = refreshTokenRequest.AccessToken;
            string? refreshToken = refreshTokenRequest.RefreshToken;

            var principal = await _authService.GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            var emailClaim = principal.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.Email);
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await _userManager.FindByEmailAsync(emailClaim.Value);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = await _authService.CreateTokenAsync(principal.Claims.ToList());
            var newRefreshToken = await _authService.GenerateRefreshTokenAsync();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);
            return new ObjectResult(new RefreshTokenResponse
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpGet]
        [Route("ua")]
        public async Task<IActionResult> Ua()
        {
            return Ok();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("revoke/{id}")]
        public async Task<IActionResult> Revoke(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return BadRequest("Invalid userId");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }
        [Authorize(AuthenticationSchemes = "Bearer")]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

    }
}
