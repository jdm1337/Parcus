using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using Parcus.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Repository
{
    public class FundsInPortfolioRepository : GenericRepository<FundsInPortfolio>, IInstrumentRepository<FundsInPortfolio>
    {
        public FundsInPortfolioRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
