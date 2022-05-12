using Microsoft.Extensions.Logging;
using Parcus.Application.Interfaces.IRepository;
using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Persistence.Data;


namespace Parcus.Persistence.Repository
{
    public class InstrumentRepository : GenericRepository<Instrument>, IInstrumentRepository
    {
        public InstrumentRepository(AppDbContext context, ILogger logger) : base(context, logger)
        {
        }
        public Task<Instrument> GetByFigi(string figi)
        {
            var instrument = _context.Instruments
                                     .Where(i => i.Figi == figi)
                                     .FirstOrDefault();
            return Task.FromResult(instrument);
        }
    }
}
