using Parcus.Domain.DTO.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.DTO.Outgoing
{
    public class GetPortfoliosResponse
    {
        public int? Count { get; set; }
        public List<PortfolioDto> Portfolios { get; set; }
    }
}
