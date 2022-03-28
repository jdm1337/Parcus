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
        Task<bool> AddBroker(BrokeragePortfolio brokeragePortfolio, string brokerName, double percentage);
        
        Task<InstrumentResult> AddTransactionAsync(InvestTransaction transaction, FinInstrumentInPortfolio instrumentInPortfolio, string instrumentType, double instrumentPrice);
    }
}
