
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Persistence.Data;

namespace Parcus.Persistence.Repository
{
    public class InstrumentsInPortfolioRepository : GenericRepository<InstrumentsInPortfolio>, IInstrumentsInPortfolioRepository
    {
        public InstrumentsInPortfolioRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<List<InstrumentsInPortfolio>> GetByPortfolioId(int portfolioId)
        {
            
            var instrumentInPortfolio = (from instrument in _context.InstrumentsInPortfolio
                                        where instrument.BrokeragePortfolioId == portfolioId
                                        select instrument).ToList();
            return instrumentInPortfolio;
        }
        public async Task<bool> UpdateAsync(InstrumentsInPortfolio instrument)
        {
            _context.Entry(await _context.InstrumentsInPortfolio
                .FirstOrDefaultAsync(i => i.Id == instrument.Id))
                .CurrentValues
                .SetValues(instrument);

            return (await _context.SaveChangesAsync()) > 0; 

        }
    }
}
