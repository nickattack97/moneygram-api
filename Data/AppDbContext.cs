using Microsoft.EntityFrameworkCore;
using moneygram_api.Models;

namespace moneygram_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<SendTransaction> SendTransactions { get; set; }
        public DbSet<CodeTable> CodeTables { get; set; }
        public DbSet<CountryInfoEntity> CountriesInfo { get; set; }
        public DbSet<CurrencyInfoEntity> CurrencyInfo { get; set; }
        public DbSet<MoneyGramXmlLog> MoneyGramXmlLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SendTransaction>(entity =>
            {
                entity.Property(e => e.SendAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReceiveAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Charge).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmountCollected).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<MoneyGramXmlLog>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Operation).IsRequired().HasMaxLength(50);
                entity.Property(e => e.RequestXml).IsRequired();
                entity.Property(e => e.ResponseXml).IsRequired();
                entity.Property(e => e.LogTime).IsRequired();
                entity.Property(e => e.Username).HasMaxLength(100);
                entity.Property(e => e.HttpMethod).HasMaxLength(10);
                entity.Property(e => e.Url).HasMaxLength(500);
            });

            modelBuilder.Entity<CodeTable>().ToTable("CodeTables");
            modelBuilder.Entity<CountryInfoEntity>().ToTable("CountriesInfo");
            modelBuilder.Entity<CurrencyInfoEntity>().ToTable("CurrencyInfo");
        }
    }
}