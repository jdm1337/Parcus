using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.InstrumentModels;
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

            if (existBroker == null)
            {
                var newBroker = await _unitOfWork.Brokers.AddAsync(new Broker
                {
                    Name = brokerName,
                    Percentage = percentage
                });
                if (newBroker == null)
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

        public async Task<Result<Broker>> AddBroker(BrokeragePortfolio brokeragePortfolio, Broker broker)
        {
            var result = new Result<Broker>();
            var existBroker = (await _unitOfWork.Brokers.GetAllAsync())
                .Where(b => b.Name == broker.Name && b.Percentage == broker.Percentage)
                .FirstOrDefault();

            if (existBroker == null)
            {
                var newBroker = await _unitOfWork.Brokers.AddAsync(broker);
                if (newBroker == null)
                {
                    result.Succeeded = false;
                    return result;
                }
                brokeragePortfolio.PortfolioBroker = broker;
            }
            else
            {
                brokeragePortfolio.PortfolioBroker = existBroker;
            }

            await _unitOfWork.Portfolios.UpdateAsync(brokeragePortfolio);
            await _unitOfWork.CompleteAsync();
            result.Succeeded = true;
            result.Item = brokeragePortfolio.PortfolioBroker;
            return result;


        }

        public async Task<Result<InvestTransaction>> AddTransactionAsync(InvestTransaction transaction)
        {
            var result = new Result<InvestTransaction>();

            if(transaction == null)  return result;

            var validationResult = await ValidateTransaction(transaction);

            if (!validationResult.Succeeded) return result;
            
            var provideTransactionResult = await ProvideTransaction(transaction);

            if (!provideTransactionResult.Succeeded) return result;
            
            result.Item = transaction;
            result.Succeeded = true;
            return result;


        }

        public async Task<ValidationResult> ValidateTransaction(InvestTransaction transaction)
        {
            var result = new ValidationResult();

            if (transaction == null || transaction.Instrument.Amount < 1 || transaction.InstrumentPrice <= 0)  return result;
            
            if(transaction.TransactionType is Transactions.Sale)
            {
                var instrumentInPortfolio = (await _unitOfWork.InstrumentsInPortfolio.GetByPortfolioId(transaction.BrokeragePortfolio.Id))
                    .Where(instrument => instrument.Figi == transaction?.Instrument?.Figi)
                    .FirstOrDefault();
                Console.WriteLine(result.Succeeded);
                if (instrumentInPortfolio == null) return result;

                else if (instrumentInPortfolio.Amount < transaction.Instrument.Amount) return result;
            }
            result.Succeeded = true;
            return result;



        }

        public async Task<Result<InvestTransaction>> ProvideTransaction(InvestTransaction transaction)
        {
            var result = new Result<InvestTransaction>();

            if (transaction == null) return result;

            var instrumentInPortfolio = (await _unitOfWork.InstrumentsInPortfolio.GetByPortfolioId(transaction.BrokeragePortfolio.Id))
                    .Where(instrument => instrument.Figi == transaction?.Instrument?.Figi)
                    .FirstOrDefault();
            
            if (transaction.TransactionType is Transactions.Buy)
            {
                Console.WriteLine("Buy in provide");
                if(instrumentInPortfolio == null)
                {
                    Console.WriteLine("instrument - new");
                    Console.WriteLine("transaction active price -" + transaction.InstrumentPrice);
                    transaction.Instrument.AveragePrice = transaction.InstrumentPrice;
                    transaction.Instrument.InvestedValue = transaction.InstrumentPrice * transaction.Instrument.Amount;
                    var addedInstrument = await _unitOfWork.InstrumentsInPortfolio.AddAsync(transaction.Instrument);

                    if (addedInstrument == null) return result;
                    transaction.Instrument = addedInstrument;
                    

                }
                else
                {
                    Console.WriteLine("buy next time");
                    instrumentInPortfolio.Amount += transaction.Instrument.Amount;
                    instrumentInPortfolio.InvestedValue += transaction.InstrumentPrice * transaction.Instrument.Amount;
                    instrumentInPortfolio.AveragePrice = instrumentInPortfolio.InvestedValue / transaction.Instrument.Amount;

                    var isUpdated = await _unitOfWork.InstrumentsInPortfolio.UpdateAsync(instrumentInPortfolio);

                    if(!isUpdated) return result;
                    transaction.Instrument = instrumentInPortfolio;
                    

                }
            }
            else
            {
                if (instrumentInPortfolio == null) return result;
                
                else
                {
                    instrumentInPortfolio.Amount -= transaction.Instrument.Amount;
                    instrumentInPortfolio.InvestedValue -= transaction.InstrumentPrice * transaction.Instrument.Amount;
                    //instrumentInPortfolio.AveragePrice = instrumentInPortfolio.InvestedValue / transaction.Instrument.Amount;

                    var isUpdated = await _unitOfWork.InstrumentsInPortfolio.UpdateAsync(instrumentInPortfolio);

                    if (!isUpdated) return result;
                    transaction.Instrument = instrumentInPortfolio;

                }
            }

            var savedTransaction = await _unitOfWork.InvestTransactions.AddAsync(transaction);
            if (savedTransaction == null) return result;

            await _unitOfWork.CompleteAsync();
            result.Succeeded = true;
            result.Item = savedTransaction;
            return result;

        }


    }
}
