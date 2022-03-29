using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.InstrumentModels.Bonds;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using Parcus.Domain.Invest.InstrumentModels.Shares;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using Parcus.Domain.Results;
using Parcus.Persistence.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Services.Services
{
    public class PortfolioOperationService : IPortfolioOperationService
    {
        protected IUnitOfWork _unitOfWork;
        public PortfolioOperationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        public async Task<bool> AddBroker(BrokeragePortfolio brokeragePortfolio, string brokerName, double percentage)
        {
            var existBroker = (await _unitOfWork.Brokers.GetAllAsync()).Where(broker => broker.Name == brokerName && broker.Percentage == percentage).FirstOrDefault();
            
            if(existBroker == null)
            {
                var newBroker = await _unitOfWork.Brokers.AddAsync(new Broker
                {
                    Name = brokerName,
                    Percentage = percentage
                });
                if(newBroker == null)
                {
                    return false;
                }
                brokeragePortfolio.PortfolioBroker = newBroker;
            }
            else
            {
                brokeragePortfolio.PortfolioBroker = existBroker;
            }
            await _unitOfWork.Portfolios.UpdateAsync(brokeragePortfolio);
            await _unitOfWork.CompleteAsync();
            return true;
        }

        public async Task<InstrumentResult> AddTransactionAsync(InvestTransaction transaction, FinInstrumentInPortfolio instrumentInPortfolio, string instrumentType, double instrumentPrice)
        {
            var result = new InstrumentResult();
            var finInstrument = new FinInstrumentInPortfolio();

            if(instrumentType == "share")
            {
                finInstrument = new SharesInPortfolio();


            }
            else if(instrumentType == "bond"){
                finInstrument = new BondsInPortfolio();

            }
            else if(instrumentType == "fund")
            {
                finInstrument = new FundsInPortfolio();
            }
            else
            {
                result.Successed = false;
                return result;
            }
            



            
        }
        private async Task<bool> ValidateTransactionAsync(int portfolioId, Transactions transactionType, FinInstrumentInPortfolio instrument)
        {
            var portfolio = await _unitOfWork.Inve
        }

    }
}
