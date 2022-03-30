using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;

namespace Parcus.Domain.Invest.InstrumentModels
{
    public class InstrumentsInPortfolio
    {
        public int Id { get; set; }
        public string? Figi { get; set; }
        public InstrumentTypes InstrumentType { get; set; }
        public int? Amount { get; set; }
        public double? AveragePrice { get; set; }
        public double? InvestedValue { get; set; }
        public double? CurrentValue { get; set; }
        public double? CurrentPrice { get; set; }
        public double? Profit { get; set; }
        public double? DailyProfit { get; set; }

        public int? BrokeragePortfolioId { get; set; }
        public BrokeragePortfolio? BrokeragePortfolio { get; set; }

        public ICollection<InvestTransaction>? Transactions { get; set; }
    }
}
