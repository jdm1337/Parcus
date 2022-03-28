using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels.Bonds;
using Parcus.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Repository
{
    public class BondsInPortfolioRepository : GenericRepository<BondsInPortfolio>, IInstrumentRepository<BondsInPortfolio>
    {
        public BondsInPortfolioRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
