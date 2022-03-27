using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
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
            var brokers = await _unitOfWork.Brokers.GetAllAsync();
            var existBroker = brokers.Where(broker => broker.Name == brokerName && broker.Percentage == percentage).FirstOrDefault();

            if(existBroker == null)
            {
                var broker = await _unitOfWork.Brokers.AddAsync(new Broker
                {
                    Name = brokerName,
                    Percentage = percentage
                });
                if(broker == null)
                {
                    return false;
                }
                brokeragePortfolio.PortfolioBroker = broker;
                await _unitOfWork.Portfolios.UpdateAsync(brokeragePortfolio);
                await _unitOfWork.CompleteAsync();
                return true;
            }
            else
            {
                brokeragePortfolio.PortfolioBroker = existBroker;
                await _unitOfWork.Portfolios.UpdateAsync(brokeragePortfolio);
                await _unitOfWork.CompleteAsync();
                return true;
            }
        }

        public Task<bool> AddTransactionAsync(InvestTransaction transaction)
        {
            // short describe of method
            // 1. Define type of
            throw new NotImplementedException();
        }

    }
}
