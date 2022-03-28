using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Persistence.Data;

namespace Parcus.Persistence.Repository
{
    public class PortfoliosRepository : GenericRepository<BrokeragePortfolio>, IPortfoliosRepository
    {
        public PortfoliosRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }

        public async Task<IEnumerable<BrokeragePortfolio>> GetByUserIdAsync(string userId)
        {
            var userPortfolios = _context.BrokeragePortfolios.Where(portfolio => portfolio.UserId.Equals(Convert.ToInt32(userId))); ;
            return await userPortfolios.ToListAsync();
        }
        public async Task<BrokeragePortfolio> GetByUserIdAndNameAsync(string userId, string name)
        {
            var userPortfolios = _context.BrokeragePortfolios.Where(portfolio => portfolio.UserId == Convert.ToInt32(userId) && portfolio.Name == name);
            var portfolio = await userPortfolios.FirstOrDefaultAsync();
            return portfolio;
        }
        public async Task<bool> UpdateAsync(BrokeragePortfolio portfolio)
        {
            _context.Entry( await _context.BrokeragePortfolios.FirstOrDefaultAsync(p => p.Id == portfolio.Id)).CurrentValues.SetValues(portfolio);
            
            return (await _context.SaveChangesAsync()) > 0;
        }
      
    }
} 
