using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Incoming
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
