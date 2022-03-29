using Microsoft.Extensions.Logging;
using Parcus.Persistence.Repository;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Application.Interfaces.IUnitOfWorkConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        //Repository of each entities keeping here.
        public IUsersRepository Users { get; private set; }
        public IPortfoliosRepository Portfolios { get; private set; }
        public IBrokersRepository Brokers { get; private set; }

        public UnitOfWork(AppDbContext context, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = loggerFactory.CreateLogger("db_logs");

            Users = new UsersRepository(context, _logger);
            Portfolios = new PortfoliosRepository(context, _logger);
            Brokers = new BrokersRepository(context, _logger);
        }

        public async Task CompleteAsync()
        {
            await _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
