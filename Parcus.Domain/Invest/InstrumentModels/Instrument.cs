using Parcus.Domain.Invest.InstrumentModels.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels
{
    public class Instrument
    {
        public int Id { get; set; }
        public string? Isin { get; set; }
        public string? Figi { get; set; }
        public string? Name { get; set; }
        public string? Tiker { get; set; }
        public string? Country { get; set; }
        public Currency? Currency { get; set; }
        public double? CurrentPrice { get; set; }

        //shares extra fields
        public double? Dividends { get; set; }
        public double? DividendYield { get; set; }
        
        //bond extra fields
        public double? CurrentProfit { get; set; }
        public double? CancelProfit { get; set; }
        public DateTime? CancelDate { get; set; }
        public double? Denomination { get; set; }
        public int? PayingPeriod { get; set; }


    }
}
