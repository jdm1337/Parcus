using Parcus.Domain.Invest.PortfolioModels;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IPortfoliosRepository : IGenericRepository<BrokeragePortfolio>
    {
        Task<IEnumerable<BrokeragePortfolio>> GetByUserIdAsync(string userId);
        Task<BrokeragePortfolio> GetByUserIdAndNameAsync(string userId, string name);
    }
}
