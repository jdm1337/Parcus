using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Domain.DTO.Entities
{
    public class InstrumentInPortfolioDto
    {
        public int Id { get; set; }
        public string? Tiker { get; set; }
        public string? Name { get; set; }
        public int? Amount { get; set; } = 0;
        public double? AveragePrice { get; set; }
        public double? InvestedValue { get; set; } = 0;
        public double? CurrentValue { get; set; } = 0;
        public double? CurrentPrice { get; set; } = 0;
        public double? Profit { get; set; } = 0;
        public double? DailyProfit { get; set; } = 0;
    }
}
