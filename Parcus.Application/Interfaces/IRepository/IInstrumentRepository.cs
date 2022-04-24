using Parcus.Domain.Invest.InstrumentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IRepository
{
    public interface IInstrumentRepository: IGenericRepository<Instrument>
    {
        public Task<Instrument> GetByFigi(string figi);
    }
}
