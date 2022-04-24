using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.InstrumentModels;


namespace Parcus.Persistence.EntityTypeConfiguration
{
    public class InstrumentInPortfolioConfiguration : IEntityTypeConfiguration<InstrumentsInPortfolio>
    {
        public void Configure(EntityTypeBuilder<InstrumentsInPortfolio> builder)
        {
            builder.HasMany(x => x.Transactions)
                .WithOne(x => x.Instrument)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
