using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.DTO.Entities;
using Parcus.Domain.DTO.Incoming;
using Parcus.Domain.DTO.Outgoing;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;
using Parcus.Domain.Settings;
using Parcus.Services.Extensions;
using System.IdentityModel.Tokens.Jwt;

namespace Parcus.Web.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;

        private const string CommonRoleName = "CommonUser";
        public AccountController(
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptionsMonitor<JwtSettings> optionsMonitor,
            IAuthService authService,
            ITokenService tokenService) : base(unitOfWork, mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = optionsMonitor.CurrentValue;
            _authService = authService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Registration of new users
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(registrationRequest.Email);

            if (existingUser != null)
                return BadRequest();
            
            var newUser = new User
            {
                Email = registrationRequest.Email,
                UserName = registrationRequest.UserName,
                EmailConfirmed = true, //TODO: build fuctionallity to send email
            };

            var isCreated = await _userManager.CreateAsync(newUser, registrationRequest.Password);

            if (!isCreated.Succeeded)
                return BadRequest();
            
            await _userManager.AddToRoleAsync(newUser, CommonRoleName);

            var createdUser = await _userManager.FindByEmailAsync(newUser.Email);
            if (createdUser == null)
                return BadRequest();

            return Ok(_mapper.Map<UserDto>(createdUser));
        }

        /// <summary>
        /// Login
        /// </summary>
        [AllowAnonymous]
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            var validPassword = await _userManager.CheckPasswordAsync(user, loginRequest.Password);
            if (user != null && validPassword)
            {
                var accessToken = await _tokenService.GenerateAccessTokenAsync(user);
                var refreshToken = await _tokenService.GenerateRefreshTokenAsync();

                await _userManager.UpdateRefreshTokenFieldsAsync(user, refreshToken, Convert.ToInt32(_jwtSettings.AccessTokenValidityInMinutes));
                return Ok(new LoginResponse
                {
                    AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    RefreshToken = refreshToken,
                    Expiration = accessToken.ValidTo
                });
            }
            return BadRequest();
        }

        /// <summary>
        /// Change password
        /// </summary>
        [Authorize(Permissions.Account.Base)]
        [HttpPost]
        [Route("Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if(request.NewPassword.Equals(request.PasswordCheck)) 
                return BadRequest("Passwords don't match.");

            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return NotFound();

            var matchPassword = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

            if (!matchPassword) 
                return BadRequest("Invalid password.");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded) 
                return BadRequest();

            return Ok(result);  
        }

        /// <summary>
        /// Get account data
        /// </summary>
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserData()
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();
            
            return Ok(_mapper.Map<UserDto>(user));
        }

        /// <summary>
        /// Get a list of portfolios
        /// </summary>
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("Portfolios")]
        public async Task<IActionResult> GetPortfolios()
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return BadRequest();
            }
            var portfolios = (await _unitOfWork.Portfolios.GetByUserIdAsync(userId))
                .Select(portfolio => _mapper.Map<PortfolioDto>(portfolio));

            if (portfolios == null)
                return BadRequest();

            var response = _mapper.Map<GetPortfoliosResponse>(portfolios);

            return Ok(response);
        }

        /// <summary>
        /// Get permissions from a user
        /// </summary>
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("Permissions")]
        public async Task<IActionResult> Permission()
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) 
                return BadRequest(); 

            var userPermissions = await _authService.GetUserPermissionsAsync(user);
            return Ok(userPermissions);
        }
    }
}
