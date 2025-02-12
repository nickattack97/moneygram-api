using Microsoft.EntityFrameworkCore;
using moneygram_api.Models;

namespace moneygram_api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<RequestLog> RequestLogs { get; set; }
        public DbSet<ExceptionLog> ExceptionLogs { get; set; }
        public DbSet<SendTransaction> SendTransactions { get; set; } 
    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SendTransaction>(entity =>
            {
                entity.Property(e => e.SendAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReceiveAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SendAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ExchangeRate).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Charge).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmountCollected).HasColumnType("decimal(18,2)");
            });
        }
    }
}