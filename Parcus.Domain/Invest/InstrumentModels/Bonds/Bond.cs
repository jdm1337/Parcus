using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels.Bonds
{
    public class Bond : FinInstrument
    {
        public string BondName { get; set; }
        public double? CurrentProfit { get; set; }
        public double? CancelProfit { get; set; }
        public DateTime? CancelDate { get; set; }
        public double? Denomination { get; set; }
        public int? PayingPeriod { get;set; }
     }
}
