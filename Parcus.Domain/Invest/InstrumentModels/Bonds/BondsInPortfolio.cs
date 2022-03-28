using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;


namespace Parcus.Domain.Invest.InstrumentModels.Bonds
{
    public class BondsInPortfolio : FinInstrumentInPortfolio
    {
        public int BrokeragePortfolioId { get; set; }
        public BrokeragePortfolio BrokeragePortfolio { get; set; }

        public ICollection<InvestTransaction> Transactions { get; set; }
    }
}
