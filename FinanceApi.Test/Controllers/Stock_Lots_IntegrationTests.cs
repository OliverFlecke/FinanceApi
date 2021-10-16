using System.Net.Mime;
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
        var request = RandomAddStockLotRequest();
        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.Add(new()
                {
                    UserId = userId,
                    Symbol = request.Symbol,
                });
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Should().ContainSingle(x =>
            x.UserId == userId
            && x.Symbol == request.Symbol
            && x.BuyDate == request.BuyDate
            && x.Shares == request.Shares
            && x.Price == request.Price,
            because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task POST_StockLot_NotAlreadyTracked_Test()
    {
        var userId = _random.Random.Next();
        var request = RandomAddStockLotRequest();
        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Should().ContainSingle(x =>
            x.UserId == userId
            && x.Symbol == request.Symbol
            && x.BuyDate == request.BuyDate
            && x.Shares == request.Shares
            && x.Price == request.Price,
            because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task POST_StockLot_WithSoldDate_Test()
    {
        var userId = _random.Random.Next();
        var request = RandomAddStockLotRequest();
        request.SoldDate = request.BuyDate + TimeSpan.FromDays(1);
        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Should().ContainSingle(x =>
            x.UserId == userId
            && x.Symbol == request.Symbol
            && x.BuyDate == request.BuyDate
            && x.SoldDate == request.SoldDate
            && x.Shares == request.Shares
            && x.Price == request.Price,
            because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task PUT_StockLot_NotFound_Test()
    {
        // Arrange
        var lotId = _random.Guid();
        var userId = _random.Random.Next();
        var client = _factory
            .MockAuth(new() { UserId = userId})
            .CreateClient();

        var body = RandomAddStockLotRequest();
        var content = new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PutAsync($"api/v1/stock/lot/{lotId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be($"Lot with id '{lotId}' was not found");
    }

    [Fact]
    public async Task PUT_StockLot_Success_Test()
    {
        // Arrange
        var lotId = _random.Guid();
        var request = RandomAddStockLotRequest();
        request.SoldDate = _random.DateTimeOffset;
        var userId = _random.Random.Next();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.Add(new()
                {
                    Symbol = request.Symbol,
                    UserId = userId,
                });
                context.StockLot.Add(new()
                {
                    Id = lotId,
                    UserId = userId,
                    Symbol = request.Symbol,
                    BuyDate = _random.DateTimeOffset,
                    SoldDate = _random.DateTimeOffset,
                    Shares = _random.Random.NextDouble(),
                    Price = _random.Random.NextDouble(),
                });

                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId})
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PutAsync($"api/v1/stock/lot/{lotId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        var lot = await context.StockLot.SingleAsync(x => x.UserId == userId);
        lot.Symbol.Should().Be(request.Symbol);
        lot.BuyDate.Should().Be(request.BuyDate);
        lot.SoldDate.Should().Be(request.SoldDate);
        lot.Shares.Should().Be(request.Shares);
        lot.Price.Should().Be(request.Price);
    }

    [Fact]
    public async Task PUT_StockLot_IsOtherUsersLot_Test()
    {
        // Arrange
        var lotId = _random.Guid();
        var request = RandomAddStockLotRequest();
        var userId = _random.Random.Next();
        var otherUserId = _random.Random.Next();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.Add(new()
                {
                    Symbol = request.Symbol,
                    UserId = otherUserId,
                });
                context.StockLot.Add(new()
                {
                    Id = lotId,
                    UserId = otherUserId,
                    Symbol = request.Symbol,
                    BuyDate = _random.DateTimeOffset,
                    SoldDate = _random.DateTimeOffset,
                    Shares = _random.Random.NextDouble(),
                    Price = _random.Random.NextDouble(),
                });

                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId})
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PutAsync($"api/v1/stock/lot/{lotId}", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be($"Lot with id '{lotId}' was not found");
    }

    AddStockLotRequest RandomAddStockLotRequest() => new AddStockLotRequest
    {
        Symbol = _random.String(),
        BuyDate = _random.DateTimeOffset,
        Shares = _random.Random.NextDouble(),
        Price = _random.Random.NextDouble(),
    };
}
