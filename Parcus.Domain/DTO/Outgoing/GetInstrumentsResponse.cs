using Parcus.Domain.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.DTO.Outgoing
{
    public class GetInstrumentsResponse
    {
        public int SharesAmount { get; set; } = 0;
        public int BondsAmount { get; set; } = 0;
        public int EtfAmount { get; set; } = 0;
        public IEnumerable<InstrumentInPortfolioDto> Shares { get; set; }
        public IEnumerable<InstrumentInPortfolioDto> Bonds { get; set; }
        public IEnumerable<InstrumentInPortfolioDto> Etf { get; set; }
    }
}
