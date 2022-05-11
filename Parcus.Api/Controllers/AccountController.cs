using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Api.Models;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Parcus.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AccountController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IOptionsMonitor<JwtSettings> optionsMonitor,
            IAuthService authService,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtSettings = optionsMonitor.CurrentValue;
            _authService = authService;
            _tokenService = tokenService;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (user == null || !validPassword)
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                return View(model);
            }
           
            await Authenticate(model.Email, role);

            var userClaims = await _authService.GetClaimsForTokenAsync(user);
            var token = await _tokenService.CreateTokenAsync(userClaims);
            
            _ = int.TryParse(_jwtSettings.RefreshTokenValidityInDays, out int refreshTokenValidityInDays);

            user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

            await _userManager.UpdateAsync(user);
            
            HttpContext.Response.Cookies.Append("access-token", new JwtSecurityTokenHandler().WriteToken(token));

            return RedirectToAction("Index", "Home");
            
        }

        private async Task Authenticate(string email, string role)
        { 
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role )
            };
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

    }
}
