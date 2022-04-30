using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Claims;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;
using Parcus.Domain.Results;
using Parcus.Domain.Settings;
using Parcus.Persistence.Data;

using System.Security.Claims;

namespace Parcus.Persistence.DataSeed
{
     public class SeedDataService : ISeedDataService
     {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly InitializeSettings _initializeSettings;
        private readonly IInstrumentStateService _instrumentStateService;

        public SeedDataService(
            IInstrumentStateService instrumentStateService,
            IOptionsMonitor<InitializeSettings> optionsMonitor,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            AppDbContext appDbContext )
        {
            _instrumentStateService = instrumentStateService;
            _initializeSettings = optionsMonitor.CurrentValue;
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }
        public async Task SeedInstrumentInfoAsync()
        {
            if (!_appDbContext.Instruments.Any())
            {
                 await _instrumentStateService.SeedInfoAsync();
            }
        }
        public async Task<Result<IdentityResult>> SeedInitIdentityAsync()
        {
            var seedResult = new Result<IdentityResult>();
            if (!_appDbContext.Users.Any())
            {
                Role adminRole = new Role 
                { 
                    Name = _initializeSettings.Role,
                    Description = "Администратор"
                };

                await _roleManager.CreateAsync(adminRole);

                adminRole = await _roleManager.FindByNameAsync(adminRole.Name);
                
                await AddPermissionsToAdminAsync(adminRole);
                
                var adminUser = new User()
                {
                    Email = _initializeSettings.Email,
                    UserName = _initializeSettings.Username,
                    EmailConfirmed = true
                };
                //seed common user role and base permissions
                var commonRole = new Role()
                {
                    Name = "CommonUser",
                    Description = "Пользователь сервиса"
                };
                await _roleManager.CreateAsync(commonRole);

                commonRole = await _roleManager.FindByNameAsync("CommonUser");

                await AddPermissionsToCommonUserAsync(commonRole);
                
                var createResult = await _userManager.CreateAsync(adminUser, _initializeSettings.Password);
                var addResult = await _userManager.AddToRoleAsync(adminUser, _initializeSettings.Role);

                if(createResult.Succeeded && addResult.Succeeded)
                {
                    seedResult.Succeeded = true;
                }
            }
            return seedResult ;
        }

        private async Task AddPermissionsToAdminAsync(Role role)
        {
            await _roleManager.AddClaimAsync(role,
                    new Claim(CustomClaimTypes.Permission, Permissions.Roles.AddPermission));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Account.Base));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Portfolios.Add));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Portfolios.Get));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Portfolios.GetInstruments));
        }
        private async Task AddPermissionsToCommonUserAsync(Role role)
        {
            await _roleManager.AddClaimAsync(role,
                    new Claim(CustomClaimTypes.Permission, Permissions.Account.Base));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Portfolios.Add));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Portfolios.Get));
            await _roleManager.AddClaimAsync(role,
                new Claim(CustomClaimTypes.Permission, Permissions.Portfolios.GetInstruments));
        }
    }
}
