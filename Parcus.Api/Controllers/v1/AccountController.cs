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
using System.IdentityModel.Tokens.Jwt;

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

        public AccountController(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork, mapper)
        {
        }

        //
        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationRequest registrationRequest)
        {
            var existingUser = await _userManager.FindByEmailAsync(registrationRequest.Email);
            if (existingUser != null)
            {
                return BadRequest();
            }
            var newUser = new User
            {
                Email = registrationRequest.Email,
                UserName = registrationRequest.UserName,
                EmailConfirmed = true, //TODO: build fuctionallity to send email
            };
            var isCreated = await _userManager.CreateAsync(newUser, registrationRequest.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest();
            }
            var createdUser = (await _userManager.FindByEmailAsync(newUser.Email));

            return Ok(_mapper.Map<UserDto>(createdUser));
        }

        //loginResponse
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
        [Authorize(Permissions.Account.ChangePassword)]
        [HttpPost]
        [Route("Password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if(request.NewPassword.Equals(request.PasswordCheck)) return BadRequest("Passwords don't match.");

            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var matchPassword = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

            if (!matchPassword) return BadRequest("Invalid password.");

            var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (!result.Succeeded) return BadRequest();

            return Ok(result);  

        }
        
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetUserData()
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<UserDto>(user));
        }
        //getportfolios response
        [HttpGet]
        [Authorize(Permissions.Account.Base)]
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
        [HttpGet]
        [Authorize(Permissions.Account.Base)]
        [Route("Portfolio/{id}")]


        //getpermissions response
        [Authorize(Permissions.Account.Base)]
        [HttpGet]
        [Route("Permissions")]
        public async Task<IActionResult> Permission()
        {
            var userId = await _authService.GetUserIdFromRequest(this.User.Identity);
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null) { return BadRequest(); }

            var userPermissions = await _authService.GetPermissionsFromUserAsync(user);
            return Ok(userPermissions);
        }


    }
}
