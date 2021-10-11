global using System;
global using System.Linq;
global using System.Threading.Tasks;
global using FinanceApi.Test.Utils;
global using FluentAssertions;
global using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        readonly SqliteConnection _connection = new("DataSource=:memory:");

        public CustomWebApplicationFactory()
        {
            _connection.Open();
        }

        public virtual new void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
            _connection.Dispose();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            base.ConfigureWebHost(builder);

            builder.ConfigureServices(services =>
            {
                if (services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<FinanceContext>)) is var dbContext
                    && dbContext is not null)
                {
                    services.Remove(dbContext);
                }

                services.AddDbContext<FinanceContext>(builder =>
                {
                    builder.UseSqlite(_connection);
                });

                using var scope = services.BuildServiceProvider().CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<FinanceContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}