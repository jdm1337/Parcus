using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.InstrumentModels.Bonds;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using Parcus.Domain.Invest.InstrumentModels.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Results
{
    public class FindInstrumentResult : InstrumentResult
    {
        public string? InstrumentType { get; set; }
        public IList<Share>? Shares { get; set; }
        public IList<Fund>? Funds { get; set; }
        public IList<Bond>? Bonds { get; set; }
    }
}
