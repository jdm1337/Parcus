using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Parcus.Persistence.EntityTypeConfiguration;
using Parcus.Application.Interfaces;
using Parcus.Domain;
using Parcus.Domain.Invest.PortfolioModels;
using Parcus.Domain.Invest.Transactions;


namespace Parcus.Persistence.Data
{
    public class AppDbContext : IdentityDbContext<User, Role, int>, IDbContext
    {
        public virtual DbSet<User> Users { get; set;}
        public virtual DbSet<BrokeragePortfolio> BrokeragePortfolios { get; set; }
        public virtual DbSet<InvestTransaction> InvestTransactions { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options) { }
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new BrokeragePortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new InvestTransactionConfiguration());
            modelBuilder.ApplyConfiguration(new SharesInPortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new FundsInPortfolioConfiguration());
            modelBuilder.ApplyConfiguration(new BondsInPortfolioConfiguration());

            base.OnModelCreating(modelBuilder);
            
        }
    }
}
