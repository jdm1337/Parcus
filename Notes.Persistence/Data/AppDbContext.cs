using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parcus.Persistence.EntityTypeConfiguration;
using Parcus.Application.Interfaces;
using Parcus.Domain.Identity;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;
using Microsoft.AspNetCore.Identity;
using Parcus.Domain.Invest.InstrumentModels;

namespace Parcus.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>,
        IdentityUserRole<int>, IdentityUserLogin<int>, RoleClaim, IdentityUserToken<int>>, IDbContext
    {
        public virtual DbSet<User> Users { get; set;}
        public virtual DbSet<BrokeragePortfolio> BrokeragePortfolios { get; set; }
        public virtual DbSet<InvestTransaction> InvestTransactions { get; set; }
        public virtual DbSet<InstrumentsInPortfolio> InstrumentsInPortfolio { get; set; }
        public virtual DbSet<Instrument> Instruments { get; set; }

     
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
           
        }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BrokeragePortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new InstrumentInPortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new CurrencyConfiguration());
            modelBuilder.ApplyConfiguration(new InstrumentConfiguration());

            base.OnModelCreating(modelBuilder);
            
        }
    }
}
