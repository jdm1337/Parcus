using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Incoming
{
    public class AddBrokerRequest
    {
        [Required]
        public string BrokerName { get; set; }
        [Required]
        public string PortfolioName { get; set; }
        [Required]
        public double Percentage { get; set; } = 0;

    }
}
