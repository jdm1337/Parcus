using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels.Bonds
{
    public class BondsInPortfolio : Bond
    {
        public int Amount { get; set; }
        public double AveragePrice { get; set; }
        public double InvestedValue { get; set; }
        public double CurrentBondsValue { get; set; }
        public double Profit { get; set; }
        public double DailyProfit { get; set; }

        public BrokeragePortfolio BrokeragePortfolio { get; set; }

        public ICollection<InvestTransaction> Transactions { get; set; }
    }
}
