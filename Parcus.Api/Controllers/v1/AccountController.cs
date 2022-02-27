using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

using Parcus.Api.Models.DTO.Incoming;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain;

namespace Parcus.Api.Controllers.v1
{
    public class AccountController : BaseController
    {
        private readonly UserManager<User> _userManager;
        public AccountController(IUnitOfWork unitOfWork, UserManager<User> userManager) : base(unitOfWork)
        {
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto registrationDto)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(new UserRegistrationRequestDto
                {

                    UserName = registrationDto.UserName
                }); 
                
            }
            Console.WriteLine("Fix1");
            var existingUser = await _userManager.FindByEmailAsync(registrationDto.Email);
            if (existingUser != null)
            {
                return BadRequest();
            }
            var newUser = new User
            {
                Email = registrationDto.Email,
                    UserName = registrationDto.Email,
                    EmailConfirmed = true, // Todo build fuctionallity to send email
            };
            var isCreated = await _userManager.CreateAsync(newUser, registrationDto.Password);

            if (!isCreated.Succeeded)
            {
                return BadRequest();
            }
            await _unitOfWork.CompleteAsync();
            return Ok();
        }

    }
}
