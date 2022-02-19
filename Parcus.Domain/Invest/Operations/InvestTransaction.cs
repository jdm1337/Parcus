using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.InstrumentModels.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using Parcus.Domain.Invest.InstrumentModels.Bonds;

namespace Parcus.Domain.Invest.Transactions
{
    public class InvestTransaction
    {
        public long Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public Transactions TransactionType { get; set; }



        
        public SharesInPortfolio? Share { get; set; }
        public FundsInPortfolio? Fund { get; set; }
        public BondsInPortfolio? Bond { get; set; }


        public int? BrokeragePortfolioId { get; set; }
        public BrokeragePortfolio? BrokeragePortfolio { get; set; }
    }
}
