using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels.Shares
{
    public class SharesInPortfolio : FinInstrumentInPortfolio
    {
        

        public ICollection<InvestTransaction> Transactions { get; set; }
        

    }
}
