using System.Text;
using System.Net.Http;
using System.Text.Json;
using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;

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
        var userId = _random.String();;
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
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.DeserializeContent<Guid>()).Should().NotBeEmpty();

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Single(x => x.UserId == userId).Should()
            .BeEquivalentTo(
                request,
                options => options.ExcludingMissingMembers(),
                because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task POST_StockLot_NotAlreadyTracked_Test()
    {
        var userId = _random.String();;
        var request = RandomAddStockLotRequest();
        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);

        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.DeserializeContent<Guid>()).Should().NotBeEmpty();

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Single(x => x.UserId == userId).Should()
            .BeEquivalentTo(
                request,
                options => options.ExcludingMissingMembers(),
                because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task POST_StockLot_WithSoldData_Test()
    {
        var userId = _random.String();;
        var request = RandomAddStockLotRequest();
        request.SoldDate = request.BuyDate + TimeSpan.FromDays(1);
        request.SoldPrice = _random.Random.NextDouble();
        request.SoldBrokerage = _random.Random.NextDouble();
        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, MediaTypeNames.Application.Json);
        // Act
        var response = await client.PostAsync("api/v1/stock/lot", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        (await response.DeserializeContent<Guid>()).Should().NotBeEmpty();

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Single(x => x.UserId == userId).Should().BeEquivalentTo(
            request,
            options => options.ExcludingMissingMembers(),
            because: "the lot should have been persisted to the database");
    }

    [Fact]
    public async Task PUT_StockLot_NotFound_Test()
    {
        // Arrange
        var lotId = _random.Guid();
        var userId = _random.String();;
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
        var userId = _random.String();;
        var symbol = _random.String();
        var request = new UpdateStockLotRequest
        {
            Shares = _random.Random.NextDouble(),
            BuyDate = _random.DateTimeOffset,
            BuyPrice = _random.Random.NextDouble(),
            BuyBrokerage = _random.Random.NextDouble(),
            SoldDate = _random.DateTimeOffset,
            SoldPrice = _random.Random.NextDouble(),
            SoldBrokerage = _random.Random.NextDouble(),
        };

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.Add(new()
                {
                    Symbol = symbol,
                    UserId = userId,
                });
                context.StockLot.Add(RandomStockLot(symbol, lotId, userId));

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
        context.StockLot.Single(x => x.UserId == userId).Should().BeEquivalentTo(
            request,
            options => options.ExcludingMissingMembers(),
            because: "the entity should be updated with the data from the request");
    }

    [Fact]
    public async Task PUT_StockLot_IsOtherUsersLot_Test()
    {
        // Arrange
        var lotId = _random.Guid();
        var userId = _random.String();;
        var request = RandomAddStockLotRequest();
        var otherUserId = _random.String();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.Add(new()
                {
                    Symbol = request.Symbol,
                    UserId = otherUserId,
                });
                context.StockLot.Add(RandomStockLot(request.Symbol, lotId, otherUserId));

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

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    public async Task GET_Lots_Test(int amount)
    {
        // Arrange
        var userId = _random.String();;
        var lots = Enumerable.Range(0, amount)
            .Select(_ => RandomStockLot(userId: userId))
            .ToList();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.AddRange(lots.Select(x => new TrackedStock
                {
                    UserId = userId,
                    Symbol = x.Symbol,
                }));
                context.StockLot.AddRange(lots);
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId})
            .CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/stock/lot");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();

        var stocks = await response.DeserializeContent<List<StockLotResponse>>();
        stocks.Should().BeEquivalentTo(lots, options => options.ExcludingMissingMembers(), because: "this is the lots that has been stored in the database for the given user");
    }

    [Fact]
    public async Task DELETE_StockLot_Test()
    {
        // Arrange
        var userId = _random.String();;
        var id = _random.Guid();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                var symbol = _random.String();
                context.Stock.Add(new()
                {
                    UserId = userId,
                    Symbol = symbol,
                });
                context.StockLot.Add(RandomStockLot(symbol, id, userId));
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/v1/stock/lot/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Where(x => x.UserId == userId).Should().BeEmpty(because: "all lots have been deleted");
    }

    [Fact]
    public async Task DELETE_LotNotFound_Test()
    {
        // Arrange
        var userId = _random.String();;
        var id = _random.Guid();

        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/v1/stock/lot/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be($"Lot with id '{id}' was not found");

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Where(x => x.UserId == userId).Should().BeEmpty(because: "no lots have every been in the database for the current user");
    }

    [Fact]
    public async Task DELETE_LotIsAnotherUsers_Test()
    {
        // Arrange
        var userId = _random.String();;
        var id = _random.Guid();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                var symbol = _random.String();
                var otherUser = _random.String();
                context.Stock.Add(new()
                {
                    UserId = otherUser,
                    Symbol = symbol,
                });
                context.StockLot.Add(RandomStockLot(symbol, id, otherUser));
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var response = await client.DeleteAsync($"api/v1/stock/lot/{id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be($"Lot with id '{id}' was not found");

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.StockLot.Where(x => x.UserId == userId).Should().BeEmpty(because: "no lots have been added for the current user");

    }

    AddStockLotRequest RandomAddStockLotRequest() => new()
    {
        Symbol = _random.String(),
        BuyDate = _random.DateTimeOffset,
        Shares = _random.Random.NextDouble(),
        BuyPrice = _random.Random.NextDouble(),
        BuyBrokerage = _random.Random.NextDouble(),
    };

    StockLot RandomStockLot(string? symbol = null, Guid? lotId = null, string? userId = null) => new()
    {
        Id = lotId ?? _random.Guid(),
        UserId = userId ?? _random.String(),
        Symbol = symbol ?? _random.String(),
        Shares = _random.Random.NextDouble(),
        BuyDate = _random.DateTimeOffset,
        BuyPrice = _random.Random.NextDouble(),
        BuyBrokerage = _random.Random.NextDouble(),
        SoldDate = _random.DateTimeOffset,
        SoldPrice = _random.Random.NextDouble(),
        SoldBrokerage = _random.Random.NextDouble(),
    };
}
