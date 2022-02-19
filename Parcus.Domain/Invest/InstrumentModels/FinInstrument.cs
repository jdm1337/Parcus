using Parcus.Domain.Invest.InstrumentModels.Currencies;
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
        public string Isin { get; set; }

        public string CompanyName { get; set; }
        public string Tiker { get; set; }
        public string Country { get; set; }
        public Currency Currency { get; set; }
        public double CurrentPrice { get; set; }
    }
}
