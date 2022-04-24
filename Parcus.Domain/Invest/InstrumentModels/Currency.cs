using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.Invest.InstrumentModels.Currencies
{
    public class Currency
    {
        public int Id { get; set; }
        public string? Country { get; set; }
        public string? Name { get; set; }
        public string? Sign { get; set; }

        
        public ICollection<Instrument>? Instruments { get; set; }
    }
}
