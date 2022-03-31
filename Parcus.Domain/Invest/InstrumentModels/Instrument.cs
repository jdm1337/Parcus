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
        public double? CurrentPrice { get; set; } = 0;

        //shares extra fields
        public double? Dividends { get; set; } = 0;
        public double? DividendYield { get; set; } = 0;

        //bond extra fields
        public double? CurrentProfit { get; set; } = 0;
        public double? CancelProfit { get; set; } = 0;
        public DateTime? CancelDate { get; set; }
        public double? Denomination { get; set; } = 0;
        public int? PayingPeriod { get; set; } = 0;


    }
}
