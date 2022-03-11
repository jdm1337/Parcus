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

       
    }
}
