using Parcus.Domain.Invest;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using Parcus.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IPortfolioOperationService
    {
        Task<Result<Broker>> AddBroker(BrokeragePortfolio brokeragePortfolio, Broker broker);
        
        Task<Result<InvestTransaction>> AddTransactionAsync(InvestTransaction transaction);
    }
}
