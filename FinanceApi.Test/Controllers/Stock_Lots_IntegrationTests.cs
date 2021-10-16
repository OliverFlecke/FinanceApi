using System.Net;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Test.Controllers;

public class Stock_Lots_IntegrationTests
{
    readonly CustomWebApplicationFactory _factory;
    readonly DataGenerator _random = new();

    public Stock_Lots_IntegrationTests(ITestOutputHelper output) => _factory = new(output);

    [Fact]
    public async Task POST_StockLot_AlreadyTracked_Test()
    {
        // Arrange
        var userId = _random.Random.Next();
        var symbol = _random.String();
        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.Add(new()
                {
                    UserId = userId,
                    Symbol = symbol,
                });
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var dto = new AddStockLotRequest
        {
            Symbol = symbol,
            BuyDate = _random.DateTimeOffset,
            Shares = _random.Random.NextDouble(),
            Price = _random.Random.NextDouble(),
        };
        var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, MediaTypeNames.Application.Json);
        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Should().ContainSingle(x =>
            x.UserId == userId
            && x.Symbol == dto.Symbol
            && x.BuyDate == dto.BuyDate
            && x.Shares == dto.Shares
            && x.Price == dto.Price,
            because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task POST_StockLot_NotAlreadyTracked_Test()
    {
        var userId = _random.Random.Next();
        var symbol = _random.String();
        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var dto = new AddStockLotRequest
        {
            Symbol = symbol,
            BuyDate = _random.DateTimeOffset,
            Shares = _random.Random.NextDouble(),
            Price = _random.Random.NextDouble(),
        };
        var content = new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, MediaTypeNames.Application.Json);
        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Should().ContainSingle(x =>
            x.UserId == userId
            && x.Symbol == dto.Symbol
            && x.BuyDate == dto.BuyDate
            && x.Shares == dto.Shares
            && x.Price == dto.Price,
            because: "the lot should have been persisted to the database");
    }
}
