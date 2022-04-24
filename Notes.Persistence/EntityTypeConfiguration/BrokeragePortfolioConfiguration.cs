using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.PortfolioModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.EntityTypeConfiguration
{
    public class BrokeragePortfolioConfiguration : IEntityTypeConfiguration<BrokeragePortfolio>
    {
        public void Configure(EntityTypeBuilder<BrokeragePortfolio> builder)
        {
            builder.HasIndex(acc => acc.Id).IsUnique();
            builder.HasMany(acc => acc.Instruments)
                   .WithOne(i => i.BrokeragePortfolio)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(acc => acc.Transactions)
                   .WithOne(Transaction => Transaction.BrokeragePortfolio)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
