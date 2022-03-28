using Parcus.Application.Interfaces.IServices;
using Parcus.Domain.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Services.Services
{
    public class FindInstrumentService : IFindInstrumentService
    {
        public async Task<FindInstrumentResult> ApiSearchAsync(string instrumentCode)
        {
            throw new NotImplementedException();
        }

        public async Task<FindInstrumentResult> GetByFigiAsync(string instrumentCode)
        {
            throw new NotImplementedException();
        }

        public async Task<FindInstrumentResult> SearchAsync(string instrumentCode)
        {
            throw new NotImplementedException();
        }
    }
}
