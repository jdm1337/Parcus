using System.ComponentModel.DataAnnotations;

namespace Parcus.Api.Models.DTO.Incoming
{
    public class AddTransactionRequest
    {
        [Required]
        public string PortfolioId { get; set; }
        [Required]
        public string Figi { get; set; }
        [Required]
        public string TransactionType { get; set; }
        [Required]
        public string TransactionDate { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public double Price { get; set;}
    }
}
