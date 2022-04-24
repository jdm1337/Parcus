using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.InstrumentModels;


namespace Parcus.Persistence.EntityTypeConfiguration
{
    public class InstrumentConfiguration : IEntityTypeConfiguration<Instrument>
    {
        public void Configure(EntityTypeBuilder<Instrument> builder)
        {
            builder.HasMany(x => x.instrumentsInPortfolios)
                .WithOne(x => x.Instrument)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
