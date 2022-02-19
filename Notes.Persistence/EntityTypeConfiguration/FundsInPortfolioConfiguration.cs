using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.InstrumentModels.Funds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.EntityTypeConfiguration
{
    public class FundsInPortfolioConfiguration :   IEntityTypeConfiguration<FundsInPortfolio>
    {
        public void Configure(EntityTypeBuilder<FundsInPortfolio> builder)
    {
        builder.HasIndex(FundsInAcc => FundsInAcc.Id).IsUnique();
        builder.HasMany(FundsInAcc => FundsInAcc.Transactions)
               .WithOne(Transaction => Transaction.Fund)
               .OnDelete(DeleteBehavior.NoAction);
    }
}
}
