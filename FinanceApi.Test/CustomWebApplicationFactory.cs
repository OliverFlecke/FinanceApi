global using System;
global using System.Linq;
global using System.Threading.Tasks;
global using FinanceApi.Test.Utils;
global using FluentAssertions;
global using Xunit;
global using Xunit.Abstractions;
global using Microsoft.EntityFrameworkCore;
global using Microsoft.Extensions.DependencyInjection;
global using MediaTypeNames = System.Net.Mime.MediaTypeNames;
using System.Collections.Generic;
using FinanceApi.Test.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FinanceApi.Test
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>, IDisposable
    {
        readonly SqliteConnection _connection = new("DataSource=:memory:");
        readonly ITestOutputHelper? _testOutputHelper;

        public CustomWebApplicationFactory(ITestOutputHelper? testOutputHelper = null)
        {
            _connection.Open();
            _testOutputHelper = testOutputHelper;
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

            if (_testOutputHelper is not null)
            {
                builder.ConfigureLogging(logBuilder =>
                    logBuilder.AddProvider(new XUnitLoggerProvider(_testOutputHelper)));
            }

            builder.ConfigureAppConfiguration((context, config) => {
                config.AddInMemoryCollection(new Dictionary<string, string>()
                {
                    ["GitHub:ClientId"] = "ontueaseou",
                    ["GitHub:ClientSecret"] = "sntohue",
                });
            });

            builder.UseEnvironment("Production");
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