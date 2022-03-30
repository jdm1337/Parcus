

namespace Parcus.Domain.Invest.InstrumentModels.Shares
{
    public class Share : Instrument
    {
        public double? Dividends { get; set; }
        public double? DividendYield { get; set; }

    }
}
