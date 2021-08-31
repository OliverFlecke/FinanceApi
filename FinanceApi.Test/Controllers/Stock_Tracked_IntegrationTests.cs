using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;
using FinanceApi.Test.Utils;
using FluentAssertions;
using Xunit;

namespace FinanceApi.Test.Controllers
{
    public class Stock_Tracked_IntegrationTests : IClassFixture<CustomWebApplicationFactory>, IClassFixture<DataGenerator>
    {
        readonly CustomWebApplicationFactory _factory;
        readonly DataGenerator _data;

        public Stock_Tracked_IntegrationTests(CustomWebApplicationFactory factory, DataGenerator data)
        {
            _factory = factory;
            _data = data;
        }

        [Fact]
        public async Task GetTrackedStocks_NoUser_Test()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/stock/tracked");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Forbidden, because: "no user is logged in");
        }

        [Fact]
        public async Task GetTrackedStocks_NoItemsListed_Test()
        {
            // Arrange
            var userId = _data.Random.Next();

            var client = _factory
                .MockAuth(new() { UserId = userId })
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/v1/stock/tracked");

            // Assert
            response.EnsureSuccessStatusCode();
            var stocks = JsonSerializer.Deserialize<List<StockDto>>(await response.Content.ReadAsStringAsync());
            stocks.Should().BeEmpty(because: "no symbols has been marked as tracked yet");
        }

        [Theory]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        [InlineData(100)]
        public async Task GetTrackedStocks_Single_Test(int numberOfTrackedSymbols)
        {
            // Arrange
            var userId = _data.Random.Next();
            var symbols = Enumerable
                .Range(0, numberOfTrackedSymbols)
                .Select(_ => _data.String())
                .ToList();

            var client = _factory
                .SetupDatabase<FinanceContext>(async context =>
                {
                    context.Stock.AddRange(symbols.Select(symbol => new TrackedStock
                    {
                        UserId = userId,
                        Symbol = symbol,
                    }));
                    await context.SaveChangesAsync();
                })
                .MockAuth(new() { UserId = userId })
                .CreateClient();

            // Act
            var response = await client.GetAsync("api/v1/stock/tracked");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var stocks = await response.DeserializeContent<List<StockDto>>();
            stocks.Should().HaveCount(numberOfTrackedSymbols, because: "user has marked this number of symbols as some they want to track");
            stocks!.Select(x => x.Symbol).Should().BeEquivalentTo(symbols);
        }
    }
}
