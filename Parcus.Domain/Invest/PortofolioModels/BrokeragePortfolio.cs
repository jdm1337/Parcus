using Parcus.Domain.Identity;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.Transactions;

namespace Parcus.Domain.Invest.PortfolioModels
{
    public class BrokeragePortfolio 
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public Broker? PortfolioBroker { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.UtcNow;
        public ICollection<InstrumentsInPortfolio>? Instruments { get; set; }
        public ICollection<InvestTransaction>? Transactions { get; set; }
        public int? UserId { get; set; }
        public User? User { get; set; } 
        
    }
}
