using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.Transactions;
using Parcus.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Repository
{
    public class InvestTransactionsRepository : GenericRepository<InvestTransaction>, IInvestTransactionsRepository
    {
        public InvestTransactionsRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
