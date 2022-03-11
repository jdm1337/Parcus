﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parcus.Persistence.Data;

#nullable disable

namespace Parcus.Persistence.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220311124701_Add some nullable for invest entities")]
    partial class Addsomenullableforinvestentities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Parcus.Domain.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Parcus.Domain.Identity.RoleClaim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Parcus.Domain.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("RefreshTokenExpiryTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Parcus.Domain.Invest.Brokers.Broker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Percentage")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("Broker");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Bonds.BondsInPortfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<double?>("AveragePrice")
                        .HasColumnType("float");

                    b.Property<string>("BondName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BrokeragePortfolioId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("CancelDate")
                        .HasColumnType("datetime2");

                    b.Property<double?>("CancelProfit")
                        .HasColumnType("float");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<double?>("CurrentBondsValue")
                        .HasColumnType("float");

                    b.Property<double?>("CurrentPrice")
                        .HasColumnType("float");

                    b.Property<double?>("CurrentProfit")
                        .HasColumnType("float");

                    b.Property<double?>("DailyProfit")
                        .HasColumnType("float");

                    b.Property<double?>("Denomination")
                        .HasColumnType("float");

                    b.Property<double?>("InvestedValue")
                        .HasColumnType("float");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("PayingPeriod")
                        .HasColumnType("int");

                    b.Property<double?>("Profit")
                        .HasColumnType("float");

                    b.Property<string>("Tiker")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BrokeragePortfolioId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("BondsInPortfolio");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Currencies.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Sign")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currency");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Funds.FundsInPortfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<double>("AveragePrice")
                        .HasColumnType("float");

                    b.Property<int>("BrokeragePortfolioId")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<double>("CurrentFundsValue")
                        .HasColumnType("float");

                    b.Property<double?>("CurrentPrice")
                        .HasColumnType("float");

                    b.Property<double>("DailyProfit")
                        .HasColumnType("float");

                    b.Property<double>("InvestedValue")
                        .HasColumnType("float");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Profit")
                        .HasColumnType("float");

                    b.Property<string>("Tiker")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BrokeragePortfolioId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("FundsInPortfolio");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Shares.SharesInPortfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("Amount")
                        .HasColumnType("int");

                    b.Property<double>("AveragePrice")
                        .HasColumnType("float");

                    b.Property<int>("BrokeragePortfolioId")
                        .HasColumnType("int");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CurrencyId")
                        .HasColumnType("int");

                    b.Property<double?>("CurrentPrice")
                        .HasColumnType("float");

                    b.Property<double>("CurrentSharesValue")
                        .HasColumnType("float");

                    b.Property<double>("DailyProfit")
                        .HasColumnType("float");

                    b.Property<double?>("DividendYield")
                        .HasColumnType("float");

                    b.Property<double?>("Dividends")
                        .HasColumnType("float");

                    b.Property<double>("InvestedValue")
                        .HasColumnType("float");

                    b.Property<string>("Isin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Profit")
                        .HasColumnType("float");

                    b.Property<string>("ShareName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Tiker")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BrokeragePortfolioId");

                    b.HasIndex("CurrencyId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("SharesInPortfolio");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PortfolioBrokerId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("PortfolioBrokerId");

                    b.HasIndex("UserId");

                    b.ToTable("BrokeragePortfolios");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.Transactions.InvestTransaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<int?>("BondId")
                        .HasColumnType("int");

                    b.Property<int?>("BrokeragePortfolioId")
                        .HasColumnType("int");

                    b.Property<int?>("FundId")
                        .HasColumnType("int");

                    b.Property<int?>("ShareId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BondId");

                    b.HasIndex("BrokeragePortfolioId");

                    b.HasIndex("FundId");

                    b.HasIndex("ShareId");

                    b.ToTable("InvestTransactions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Parcus.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Parcus.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Parcus.Domain.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parcus.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Parcus.Domain.Identity.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Parcus.Domain.Identity.RoleClaim", b =>
                {
                    b.HasOne("Parcus.Domain.Identity.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Bonds.BondsInPortfolio", b =>
                {
                    b.HasOne("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", "BrokeragePortfolio")
                        .WithMany("Bonds")
                        .HasForeignKey("BrokeragePortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parcus.Domain.Invest.InstrumentModels.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.Navigation("BrokeragePortfolio");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Funds.FundsInPortfolio", b =>
                {
                    b.HasOne("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", "BrokeragePortfolio")
                        .WithMany("Funds")
                        .HasForeignKey("BrokeragePortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parcus.Domain.Invest.InstrumentModels.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.Navigation("BrokeragePortfolio");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Shares.SharesInPortfolio", b =>
                {
                    b.HasOne("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", "BrokeragePortfolio")
                        .WithMany("Shares")
                        .HasForeignKey("BrokeragePortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parcus.Domain.Invest.InstrumentModels.Currencies.Currency", "Currency")
                        .WithMany()
                        .HasForeignKey("CurrencyId");

                    b.Navigation("BrokeragePortfolio");

                    b.Navigation("Currency");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", b =>
                {
                    b.HasOne("Parcus.Domain.Invest.Brokers.Broker", "PortfolioBroker")
                        .WithMany()
                        .HasForeignKey("PortfolioBrokerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Parcus.Domain.Identity.User", "User")
                        .WithMany("BrokPortfolios")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PortfolioBroker");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.Transactions.InvestTransaction", b =>
                {
                    b.HasOne("Parcus.Domain.Invest.InstrumentModels.Bonds.BondsInPortfolio", "Bond")
                        .WithMany("Transactions")
                        .HasForeignKey("BondId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", "BrokeragePortfolio")
                        .WithMany("Transactions")
                        .HasForeignKey("BrokeragePortfolioId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Parcus.Domain.Invest.InstrumentModels.Funds.FundsInPortfolio", "Fund")
                        .WithMany("Transactions")
                        .HasForeignKey("FundId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Parcus.Domain.Invest.InstrumentModels.Shares.SharesInPortfolio", "Share")
                        .WithMany("Transactions")
                        .HasForeignKey("ShareId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.Navigation("Bond");

                    b.Navigation("BrokeragePortfolio");

                    b.Navigation("Fund");

                    b.Navigation("Share");
                });

            modelBuilder.Entity("Parcus.Domain.Identity.User", b =>
                {
                    b.Navigation("BrokPortfolios");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Bonds.BondsInPortfolio", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Funds.FundsInPortfolio", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.InstrumentModels.Shares.SharesInPortfolio", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("Parcus.Domain.Invest.PortfolioModels.BrokeragePortfolio", b =>
                {
                    b.Navigation("Bonds");

                    b.Navigation("Funds");

                    b.Navigation("Shares");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
