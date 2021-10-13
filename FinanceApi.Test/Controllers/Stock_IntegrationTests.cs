using System.Text.Json;
using System.Collections.Generic;
using FinanceApi.Areas.Stocks.Dtos;
using System.Net;
using FinanceApi.Utils;

namespace FinanceApi.Test
{
    public class Stock_IntegrationTests
    {
        readonly CustomWebApplicationFactory _factory = new();


        [Fact]
        public async Task GetStockSymbol_NoSymbols_Test()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/stock");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest, because: "no symbols has been provided");
            var content = await response.DeserializeContent<YahooError>();
            content!.Description.Should().Be("Missing required query parameter=symbols", because: "no symboles has been provided");
        }

        [Fact]
        public async Task GetStockSymbol_Single_Test()
        {
            // Arrange
            var client = _factory.CreateClient();
            var uri = new UriBuilder
            {
                Path = "/api/v1/stock",
                Query = "symbols=AAPL",
            };

            // Act
            var response = await client.GetAsync(uri.ToString());

            // Assert
            response.EnsureSuccessStatusCode();
            var stocks = JsonSerializer.Deserialize<List<StockDto>>(await response.Content.ReadAsStringAsync());
            stocks.Should().HaveCount(1, because: "we only asked for one symbol");
        }

        [Fact]
        public async Task GetStockSymbol_Multiple_Test()
        {
            // Arrange
            var client = _factory.CreateClient();
            var uri = new UriBuilder
            {
                Path = "/api/v1/stock",
                Query = "symbols=AAPL,MSFT",
            };

            // Act
            var response = await client.GetAsync(uri.ToString());

            // Assert
            response.EnsureSuccessStatusCode();
            var stocks = JsonSerializer.Deserialize<List<StockDto>>(await response.Content.ReadAsStringAsync());
            stocks.Should().HaveCount(2, because: "we asked for multiple symbols");
        }
    }
}
