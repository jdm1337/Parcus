using System.ComponentModel.DataAnnotations;

namespace Parcus.Domain.DTO.Incoming
{
    public class AddBrokerRequest
    {
        [Required]
        [MaxLength(64)]
        public string BrokerName { get; set; }
        [Required]
        public string PortfolioId { get; set; }    
        [Required]
        public double Percentage { get; set; } = 0;

    }
}
