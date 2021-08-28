using System;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.Generic;
using FinanceApi.Areas.Stocks.Dtos;
using FluentAssertions;
using Xunit;

namespace FinanceApi.Test
{
    public class StockIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        readonly CustomWebApplicationFactory _factory;

        public StockIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetStockSymbol_NoSymbols_Test()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/stock");

            // Assert
            response.EnsureSuccessStatusCode();
            var stocks = JsonSerializer.Deserialize<List<StockDto>>(await response.Content.ReadAsStringAsync());
            stocks.Should().BeEmpty(because: "no symboles has been provided");
        }
    }
}
