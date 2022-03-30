using Parcus.Domain.Invest.InstrumentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IInstrumentsInPortfolioRepository : IGenericRepository<InstrumentsInPortfolio>
    {
        public Task<List<InstrumentsInPortfolio>> GetByPortfolioId(int portfolioId);
        public Task<bool> UpdateAsync(InstrumentsInPortfolio instrument);
        
    }
}
