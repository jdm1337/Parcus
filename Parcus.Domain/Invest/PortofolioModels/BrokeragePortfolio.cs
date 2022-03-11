using Parcus.Domain.Identity;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.InstrumentModels.Bonds;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using Parcus.Domain.Invest.InstrumentModels.Shares;
using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.PortfolioModels
{
    public class BrokeragePortfolio 
    {
        public int Id { get; set; }
        public Broker PortfolioBroker { get; set; }

        //public ICollection<>
        public ICollection<SharesInPortfolio>? Shares { get; set; }
        public ICollection<FundsInPortfolio>? Funds { get; set; }
        public ICollection<BondsInPortfolio> Bonds { get; set; }


        //Transaction history
        public ICollection<InvestTransaction>? Transactions { get; set; }


        public int UserId { get; set; }
        public User User { get; set; } 
        
    }
}
