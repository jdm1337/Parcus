using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Outgoing
{
    public class RefreshTokenResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
