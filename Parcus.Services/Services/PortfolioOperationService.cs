using Parcus.Application.Interfaces.IServices;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using Parcus.Domain.Invest.Brokers;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using Parcus.Domain.Results;
using Parcus.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace Parcus.Services.Services
{
    public class PortfolioOperationService : IPortfolioOperationService
    {
        protected IUnitOfWork _unitOfWork;
        protected AppDbContext _context;
        public PortfolioOperationService(IUnitOfWork unitOfWork, AppDbContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }
        public async Task<Result<Broker>> AddBroker(BrokeragePortfolio brokeragePortfolio, Broker broker)
        {
            var result = new Result<Broker>();
            //Edit Implementation
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

            if(transaction == null)  
                return result;

            var validationResult = await ValidateTransactionAsync(transaction);

            if (!validationResult.Succeeded) 
                return result;
            
            var provideTransactionResult = await ProvideTransaction(transaction);

            if (!provideTransactionResult.Succeeded) 
                return result;
            
            result.Item = transaction;
            result.Succeeded = true;
            return result;
        }
        public async Task<ValidationResult> ValidateTransactionAsync(InvestTransaction transaction)
        {
            var result = new ValidationResult();

            if (transaction == null || 
                transaction.InstrumentAmount < 1 || 
                transaction.InstrumentPrice <= 0 ||
                transaction.Date == null ||
                transaction.Type == null)

                return result;
            
            if(transaction.Type is Transactions.Sale)
            {
                var instrumentInPortfolio = await (_context.InstrumentsInPortfolio
                    .Where(x => x.BrokeragePortfolioId == transaction.BrokeragePortfolioId)
                    .Where(x => x.InstrumentId == transaction.Instrument.InstrumentId)
                    .FirstOrDefaultAsync()); 
                
                if (instrumentInPortfolio == null) 
                    return result;

                else if (instrumentInPortfolio.Amount < transaction.InstrumentAmount) 
                    return result;
            }
            result.Succeeded = true;
            return result;
        }
        public async Task<Result<InvestTransaction>> ProvideTransaction(InvestTransaction transaction)
        {
            var result = new Result<InvestTransaction>();

            if (transaction == null) 
                return result;

            var instrumentInPortfolio = await (_context.InstrumentsInPortfolio
                    .Where(x => x.BrokeragePortfolioId == transaction.BrokeragePortfolioId)
                    .Where(x => x.InstrumentId == transaction.Instrument.InstrumentId)
                    .FirstOrDefaultAsync());

            if (transaction.Type is Transactions.Buy)
            {
                if(instrumentInPortfolio == null)
                {
                    transaction.Instrument.Amount = transaction.InstrumentAmount;
                    transaction.Instrument.AveragePrice = transaction.InstrumentPrice;
                    transaction.Instrument.InvestedValue = transaction.InstrumentPrice * transaction.InstrumentAmount;
                    var addedInstrument = await _unitOfWork.InstrumentsInPortfolio.AddAsync(transaction.Instrument);

                    if (addedInstrument == null) return result;
                    transaction.Instrument = addedInstrument;
                }
                else
                {
                    instrumentInPortfolio.Amount += transaction.InstrumentAmount;
                    instrumentInPortfolio.InvestedValue += transaction.InstrumentPrice * transaction.InstrumentAmount;
                    instrumentInPortfolio.AveragePrice = instrumentInPortfolio.InvestedValue / instrumentInPortfolio.Amount;

                    var isUpdated = await _unitOfWork.InstrumentsInPortfolio.UpdateAsync(instrumentInPortfolio);

                    if(!isUpdated) 
                        return result;
                    transaction.Instrument = instrumentInPortfolio;
   
                }
            }
            else
            {
                instrumentInPortfolio.Amount -= transaction.InstrumentAmount;
                instrumentInPortfolio.InvestedValue -= transaction.InstrumentPrice * transaction.InstrumentAmount;
                instrumentInPortfolio.AveragePrice = instrumentInPortfolio.InvestedValue / instrumentInPortfolio.Amount;

                var isUpdated = await _unitOfWork.InstrumentsInPortfolio.UpdateAsync(instrumentInPortfolio);

                if (!isUpdated)
                    return result;
                transaction.Instrument = instrumentInPortfolio;
                
            }

            var savedTransaction = await _unitOfWork.InvestTransactions.AddAsync(transaction);
            if (savedTransaction == null) 
                return result;

            await _unitOfWork.CompleteAsync();
            result.Succeeded = true;
            result.Item = savedTransaction;

            return result;

        }


    }
}
