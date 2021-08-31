using FinanceApi.Areas.Stocks.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi
{
    public class FinanceContext : DbContext
    {
#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public FinanceContext(DbContextOptions<FinanceContext> options)
#pragma warning restore CS8618 // Consider declaring the property as nullable.
            : base(options)
        {
        }

        public DbSet<TrackedStock> Stock { get; set; }

        public DbSet<StockLot> StockLot { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrackedStock>(entity =>
            {
                entity.HasKey(x => new { x.UserId, x.Symbol });
            });

            modelBuilder.Entity<StockLot>(entity =>
            {
                entity
                    .HasOne(x => x.TrackedSymbol)
                    .WithMany()
                    .HasForeignKey(x => new { x.UserId, x.Symbol });
            });
        }
    }
}
