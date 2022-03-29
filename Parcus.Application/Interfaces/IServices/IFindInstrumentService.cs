using Parcus.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IFindInstrumentService
    {
        
        Task<FindInstrumentResult> GetByFigiAsync(string instrumentCode);
        Task<FindInstrumentResult> GetTypeByFigiAsync(string instrumentCode);
        
        //Task<string> GetTypeByFigiAsync(string instrumentCode);


    }
}
