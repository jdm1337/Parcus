using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IDataInstrumentService
    {
        Task<Result<InstrumentTypes>> DefineTypeByFigi(string figi);
        Task<Result<string>> DefineInstrumentNameByFigi (string figi);
        
    }
}
