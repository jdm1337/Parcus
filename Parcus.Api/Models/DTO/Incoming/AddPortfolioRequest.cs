using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Incoming
{
    public class AddPortfolioRequest
    {
        [Required]
        public string PortfolioName { get; set; }
    }
}
