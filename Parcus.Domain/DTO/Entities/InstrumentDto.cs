using Parcus.Domain.Invest.InstrumentModels;
using Parcus.Domain.Invest.InstrumentModels.Currencies;

namespace Parcus.Domain.DTO.Entities
{
    public class InstrumentDto
    {
        
        public string? Isin { get; set; }
        public InstrumentTypes? Type { get; set; }
        public string? Figi { get; set; }
        public string? Name { get; set; }
        public string? Tiker { get; set; }
        public string? Country { get; set; }
        public Currency? Currency { get; set; }
        public double? CurrentPrice { get; set; } = 0;
    }
}
