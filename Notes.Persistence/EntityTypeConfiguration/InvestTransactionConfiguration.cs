using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Parcus.Domain.Invest.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parcus.Persistence.EntityTypeConfiguration
{
    
        public class InvestTransactionConfiguration : IEntityTypeConfiguration<InvestTransaction>
        {
            public void Configure(EntityTypeBuilder<InvestTransaction> builder)
            {
           
            }
        }
}
