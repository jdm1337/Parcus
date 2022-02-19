using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.InstrumentModels.Bonds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.EntityTypeConfiguration
{
    public class BondsInPortfolioConfiguration : IEntityTypeConfiguration<BondsInPortfolio>
    {
        public void Configure(EntityTypeBuilder<BondsInPortfolio> builder)
        {
            builder.HasIndex(BondsInAcc => BondsInAcc.Id).IsUnique();
            builder.HasMany(BondsInAcc => BondsInAcc.Transactions)
                   .WithOne(Transaction => Transaction.Bond)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
