using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels.Funds
{
    public class FundsInPortfolio : FinInstrumentInPortfolio
    {
        

        public int BrokeragePortfolioId { get; set; }

        public BrokeragePortfolio BrokeragePortfolio { get; set; }

        public ICollection<InvestTransaction> Transactions { get; set; }
    }
}
