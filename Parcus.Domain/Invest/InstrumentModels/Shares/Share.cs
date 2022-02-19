using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Parcus.Domain.Invest.InstrumentModels.Currencies;

namespace Parcus.Domain.Invest.InstrumentModels.Shares
{
    public class Share : FinInstrument
    {
        public string ShareName { get; set;}
        
        public double Dividends { get; set; }
        public double DividendYield { get; set; }

    }
}
