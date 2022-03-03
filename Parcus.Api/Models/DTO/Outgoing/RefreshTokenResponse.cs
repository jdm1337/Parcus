using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Outgoing
{
    public class RefreshTokenResponse
    {
        [Required]
        public string? AccessToken { get; set; }
        [Required]
        public string? RefreshToken { get; set; }
    }
}
