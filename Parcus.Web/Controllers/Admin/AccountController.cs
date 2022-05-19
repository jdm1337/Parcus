using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Parcus.Web.Models;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Identity;
using Parcus.Domain.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Parcus.Web.Controllers.Admin
{
    public class AccountController : BaseAdminController
    {
        private readonly UserManager<User> _userManager;
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        public AccountController(
            UserManager<User> userManager,
            IAuthService authService,
            ITokenService tokenService)
        {
            _userManager = userManager;
            _authService = authService;
            _tokenService = tokenService;
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user == null)
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                return View(model);
            }
            var validPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!validPassword)
            {
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
                return View(model);
            }
            var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            await AuthenticateAsync(model.Email, role);

            var token = await _tokenService.GenerateAccessTokenAsync(user);

            HttpContext.Response.Cookies.Append("access-token", new JwtSecurityTokenHandler().WriteToken(token));
            

            return RedirectToAction("Index", "Home");  
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public async Task<IActionResult> AccessDenied()
        {
            return View();
        }
        private async Task AuthenticateAsync(string email, string role)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, email),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role )
            };
            ClaimsIdentity id = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType
                );
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
    }
}
