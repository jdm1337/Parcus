
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels
{
    public class FinInstrument
    {
        public int Id { get; set; }
        public string? Isin { get; set; }
        public string? Figi { get; set; }

        public string? CompanyName { get; set; }
        public string? Tiker { get; set; }
        public string? Country { get; set; }
        public string? Currency { get; set; }
        public double? CurrentPrice { get; set; }
    }
}
