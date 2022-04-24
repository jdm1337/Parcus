using Parcus.Domain.Invest.InstrumentModels;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IInstrumentsInPortfolioRepository : IGenericRepository<InstrumentsInPortfolio>
    {
        
        public Task<IEnumerable<InstrumentsInPortfolio>> GetByPortfolioId(int portfolioId);
        
       
        public Task<bool> UpdateAsync(InstrumentsInPortfolio instrument);
        
    }
}
