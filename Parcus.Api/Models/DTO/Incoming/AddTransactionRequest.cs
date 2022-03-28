namespace Parcus.Api.Models.DTO.Incoming
{
    public class AddTransactionRequest
    {
        public string PortfolioName { get; set; }
        public string Figi { get; set; }
        public string TransactionType { get; set; }
        public string TransactionDate { get; set; }
        public int Amount { get; set; }
        public double Price { get; set;}
    }
}
