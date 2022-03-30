using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Persistence.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.Repository
{
    public class InstrumentRepository : GenericRepository<Instrument>, IInstrumentRepository
    {
        public InstrumentRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
    }
}
