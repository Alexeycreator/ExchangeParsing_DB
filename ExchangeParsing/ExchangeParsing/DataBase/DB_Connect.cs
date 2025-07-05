using System;
using System.Data.Entity;
using ExchangeParsing.MoscowExchange.Models;
using ExchangeParsing.CentralBank;
using System.Configuration;
using ExchangeParsing.DataBase.Tables;

namespace ExchangeParsing.DataBase
{
  internal sealed class DB_Connect : DbContext
  {
    public DbSet<Bond> Bonds { get; set; }

    public DbSet<Stock> Stocks { get; set; }

    public DbSet<CurrencyModel> Currencies { get; set; }

    public DbSet<Client> Clients { get; set; }

    public DbSet<Address> Addresses { get; set; }

    public DbSet<HistoryPortfolio> HistoryPortfolios { get; set; }

    public DbSet<Portfolio_Currency> Portfolio_Currencies { get; set; }

    public DbSet<SecurityPortfolio> SecurityPortfolios { get; set; }

    public DB_Connect()
    {
      string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
      this.Database.Connection.ConnectionString = connectionString ?? throw new InvalidOperationException($"Строка подключения не найдена в App.config");
    }

    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //  base.OnModelCreating(modelBuilder);
    //  modelBuilder.Entity<Portfolio_Currency>().HasKey(pc => new { pc.Portfolio_Id, pc.Currency_Id });
    //  modelBuilder.Entity<Portfolio_Currency>()
    //    .HasRequired(pc => pc.SecurityPortfolio)
    //    .WithMany()
    //    .HasForeignKey(pc => pc.Portfolio_Id);

    //  modelBuilder.Entity<Portfolio_Currency>()
    //      .HasRequired(pc => pc.CurrencyModel)
    //      .WithMany()
    //      .HasForeignKey(pc => pc.Currency_Id);
    //}
  }
}
