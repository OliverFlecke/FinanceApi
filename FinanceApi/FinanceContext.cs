using FinanceApi.Areas.Account.Models;
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

        public DbSet<Account> Account { get; set; }

        public DbSet<AccountEntry> AccountEntry { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder
                .UseSnakeCaseNamingConvention());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Stock configuration
            modelBuilder.Entity<TrackedStock>(entity =>
            {
                entity.HasKey(x => new { x.UserId, x.Symbol });
            });

            modelBuilder.Entity<StockLot>(entity =>
            {
                entity
                    .HasOne(lot => lot.TrackedSymbol)
                    .WithMany(stock => stock.Lots)
                    .HasForeignKey(x => new { x.UserId, x.Symbol })
                    .HasPrincipalKey(x => new { x.UserId, x.Symbol });
            });

            // Account configuration
            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasKey(account => account.Id);
                entity.HasIndex(account => new { account.UserId, account.Name });
            });

            modelBuilder.Entity<AccountEntry>(entity =>
            {
                entity.HasKey(entry => new { entry.AccountId, entry.Date });
                entity
                    .HasOne(entry => entry.Account)
                    .WithMany(account => account.Entries)
                    .HasForeignKey(entry => entry.AccountId);
            });
        }
    }
}
