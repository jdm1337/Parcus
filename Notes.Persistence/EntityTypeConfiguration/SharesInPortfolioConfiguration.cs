using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.InstrumentModels.Shares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.EntityTypeConfiguration
{
    public class SharesInPortfolioConfiguration : IEntityTypeConfiguration<SharesInPortfolio>
    {
        public void Configure(EntityTypeBuilder<SharesInPortfolio> builder)
        {
            builder.HasIndex(SharesInAcc => SharesInAcc.Id).IsUnique();
            builder.HasMany(SharesInAcc => SharesInAcc.Transactions)
                   .WithOne(Transaction => Transaction.Share)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
