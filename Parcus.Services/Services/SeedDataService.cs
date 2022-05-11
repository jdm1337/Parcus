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
                await _instrumentStateService.SeedInfoAsync();
            
        }
        public async Task SeedInitIdentityAsync()
        {
            if (!_appDbContext.Users.Any())
            {
                //seed admin user and permission for them
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
                var commonRoleSetup = new Role()
                {
                    Name = "CommonUser",
                    Description = "Пользователь сервиса"
                };
                await _roleManager.CreateAsync(commonRoleSetup);

                var commonRole = await _roleManager.FindByNameAsync("CommonUser");

                await AddPermissionsToCommonUserAsync(commonRole);

                //seed demo role
                var demoRoleSetup = new Role()
                {
                    Name = "DemoUser",
                    Description = "Роль для демонстрации админ панели"
                };
                await _roleManager.CreateAsync(demoRoleSetup);
                var demoRole = await _roleManager.FindByNameAsync("DemoUser");
                await AddPermissionsToDemoUserAsync(demoRole);

                var demoUser = new User()
                {
                    Email = "demo@demo.com",
                    UserName = "DemoUser",
                    EmailConfirmed = true
                };

                // create user and add them to admin role
                await _userManager.CreateAsync(adminUser, _initializeSettings.Password);
                await _userManager.AddToRoleAsync(adminUser, _initializeSettings.Role);

                //create user and add them to demo role
                await _userManager.CreateAsync(demoUser, "Qwer_1234");
                await _userManager.AddToRoleAsync(demoUser, demoRole.Name); 

            }        
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
        private async Task AddPermissionsToDemoUserAsync(Role role)
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
