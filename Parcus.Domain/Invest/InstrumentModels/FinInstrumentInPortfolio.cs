using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels
{
    public class FinInstrumentInPortfolio
    {
        public int Id { get; set; }
        public string? Figi { get; set; }
        public int? Amount { get; set; }
        public double? AveragePrice { get; set; }
        public double? InvestedValue { get; set; }
        public double? CurrentFundsValue { get; set; }
        public double? Profit { get; set; }
        public double? DailyProfit { get; set; }
    }
}
