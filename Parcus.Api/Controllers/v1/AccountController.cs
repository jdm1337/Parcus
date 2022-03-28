using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Domain.Permission;
using Parcus.Api.Models.DTO.Incoming;
using Parcus.Api.Models.DTO.Outgoing;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain;

using Parcus.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Parcus.Domain.Identity;

namespace Parcus.Api.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AccountController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork,
            IOptionsMonitor<JwtSettings> optionsMonitor,
            IAuthService authService,
            ITokenService tokenService) : base(unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = optionsMonitor.CurrentValue;
            _authService = authService;
            _tokenService = tokenService;
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
                UserName = registrationRequest.UserName,
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
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
            {
                var userClaims = await _authService.GetUsersClaimsForTokenAsync(user);
                var token = await _tokenService.CreateTokenAsync(userClaims);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();
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
        [Authorize(Permissions.Users.GetUser)]
        [HttpGet]
        [Route("")]
         public async Task<IActionResult> GetUserData()
        {
            
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            return Ok(new UserDataResponse
            {
                UserId = Convert.ToString(user.Id),
                Email = user.Email,
                Username = user.UserName
            });
        }
        
    }
}
