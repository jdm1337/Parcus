using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Incoming
{
    public class UserRegistrationRequestDto
    {
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }


    }
}
