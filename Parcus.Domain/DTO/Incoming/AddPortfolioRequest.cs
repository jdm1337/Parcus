using System.ComponentModel.DataAnnotations;

namespace Parcus.Domain.DTO.Incoming
{
    public class AddPortfolioRequest
    {
        [Required]
        [MaxLength(64)]
        public string Name { get; set; }
    }
}
