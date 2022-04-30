using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Application.Interfaces.IServices
{
    public interface IHangfireInjectService
    { 
        void Initial();    
        void UpdateInstrumentsAndCalcPortfolio();
        void UpdateInstrumentsStatementJob();
    }
}
