using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.PortfolioModels;


namespace Parcus.Domain.Invest.Transactions
{
    public class InvestTransaction
    {
        public long Id { get; set; }
        public DateTime? TransactionDate { get; set; }
        public Transactions? TransactionType { get; set; }


        public int? InstrumentsInPortfolioId { get; set; }   
        public InstrumentsInPortfolio? Instrument { get; set; }
        public double? InstrumentPrice { get; set; }
        public int? Amount { get; set; }
       
        public int? BrokeragePortfolioId { get; set; }
        public BrokeragePortfolio? BrokeragePortfolio { get; set; }
    }
}
