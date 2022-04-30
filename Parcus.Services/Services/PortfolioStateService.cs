using Microsoft.EntityFrameworkCore;
using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Persistence.Data;


namespace Parcus.Services.Services
{
    public class PortfolioStateService : IPortfolioStateService
    {
        private readonly AppDbContext _context;
        public PortfolioStateService(AppDbContext context)
        {
            _context = context;
        }
        public void CalculateFields()
        {
            var instrumentInPortfolioList = _context.InstrumentsInPortfolio
                .Where(x => x.Amount > 0)
                .Include(x => x.Instrument)
                .Select(x => new InstrumentsInPortfolio
                {
                    Id = x.Id,
                    Instrument = x.Instrument,
                    Amount = x.Amount,
                    InvestedValue = x.InvestedValue,
                    CurrentValue = x.CurrentValue,
                    Profit = x.Profit
                })
                .Select(x => new InstrumentsInPortfolio
                {
                    Id = x.Id,
                    Instrument = x.Instrument,
                    Amount = x.Amount,
                    CurrentValue = x.CurrentValue,
                    InvestedValue = CalculateCurrentValue(x),
                    Profit = CalculateProfit(x),
                })
               .ToList();

            _context.InstrumentsInPortfolio.BulkUpdate(instrumentInPortfolioList, options =>
            {
                options.ColumnInputExpression = instrument => new { instrument.Id, instrument.Amount, instrument.InvestedValue, instrument.CurrentValue, instrument.Profit };
                options.ColumnPrimaryKeyExpression = instrument => instrument.Id;
            });

        }
        private static double CalculateCurrentValue(InstrumentsInPortfolio instrumentsInPortfolio)
        {
            return (double)(instrumentsInPortfolio.Instrument.CurrentPrice * instrumentsInPortfolio.Amount);
        }
        private static double CalculateProfit(InstrumentsInPortfolio instrumentsInPortfolio)
        {
            return (double) ((instrumentsInPortfolio.CurrentValue - instrumentsInPortfolio.InvestedValue) / 100);
        }
    }
}
