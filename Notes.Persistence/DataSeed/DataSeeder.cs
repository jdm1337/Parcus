using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using Parcus.Domain.Claims;
using Parcus.Domain.Identity;
using Parcus.Domain.Permission;
using Parcus.Domain.Settings;
using Parcus.Persistence.Data;

using System.Security.Claims;



namespace Parcus.Persistence.DataSeed
{
     public class DataSeeder
    {
        private readonly AppDbContext _appDbContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly InitializeSettings _initializeSettings;


        public DataSeeder(
            IOptionsMonitor<InitializeSettings> optionsMonitor,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            AppDbContext appDbContext )
        {
            _initializeSettings = optionsMonitor.CurrentValue;
            _userManager = userManager;
            _roleManager = roleManager;
            _appDbContext = appDbContext;
        }
        public async Task<bool> Seed()
        {
            var isSeed = true;
            if (!_appDbContext.Users.Any())
            {
                Role role = new Role 
                { 
                    Name = _initializeSettings.Role,
                    Description = "FFF"
                };
                var result = await _roleManager.CreateAsync(role);
                Console.WriteLine(result.Succeeded);
                
                var adminRole = await _roleManager.FindByNameAsync(_initializeSettings.Role);
                
                await _roleManager.AddClaimAsync(adminRole,
                    new Claim(CustomClaimTypes.Permission, Permissions.Roles.AddPermission));
                
                var adminUser = new User()
                {
                    Email = _initializeSettings.Email,
                    UserName = _initializeSettings.Username,
                    EmailConfirmed = true
                };
                
                await _userManager.CreateAsync(adminUser, _initializeSettings.Password);
                await _userManager.AddToRoleAsync(adminUser, _initializeSettings.Role);
                return isSeed;
            }
            return !isSeed ;
        }
    }
}
