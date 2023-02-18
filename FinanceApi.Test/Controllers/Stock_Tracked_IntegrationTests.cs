using System.Text;
using System.Net.Http;
using System.Text.Json;
using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Test.Controllers;

public class Stock_Tracked_IntegrationTests : IClassFixture<DataGenerator>
{
    readonly CustomWebApplicationFactory _factory = new();
    readonly DataGenerator _data;

    public Stock_Tracked_IntegrationTests(DataGenerator data)
    {
        _data = data;
    }

    [Fact]
    public async Task GetTrackedStocks_NoUser_Test()
    {
        // Arrange
        var client = _factory.CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/stock/tracked");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, because: "no user is logged in");
    }

    [Fact]
    public async Task GetTrackedStocks_NoItemsListed_Test()
    {
        // Arrange
        var userId = _data.String();;

        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/stock/tracked");

        // Assert
        response.EnsureSuccessStatusCode();
        var stocks = JsonSerializer.Deserialize<List<StockResponse>>(await response.Content.ReadAsStringAsync());
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
        var userId = _data.String();;
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
        var stocks = await response.DeserializeContent<List<StockResponse>>();
        stocks.Should().HaveCount(numberOfTrackedSymbols, because: "user has marked this number of symbols as some they want to track");
        stocks!.Select(x => x.Symbol).Should().BeEquivalentTo(symbols);
    }

    [Fact]
    public async Task AddStockAsTracked_NoUser_Test()
    {
        var client = _factory.CreateClient();

        // Act
        var response = await client.PostAsync("api/v1/stock/tracked", null!);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized, because: "no user is logged in");
    }

    [Fact]
    public async Task POST_AddTrackedStock_Test()
    {
        // Arrange
        var userId = _data.String();;
        var symbol = _data.String();

        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var content = new StringContent(symbol, Encoding.UTF8, MediaTypeNames.Text.Plain);
        var response = await client.PostAsync("api/v1/stock/tracked", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var db = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        db.Stock.Should().ContainSingle(
            x => x.UserId == userId && x.Symbol == symbol,
            because: "this is the symbol which the given user has added");
    }

    [Fact]
    public async Task POST_AddTrackedStock_WhenStockIsAlreadyTracked_Test()
    {
        // Arrange
        var userId = _data.String();;
        var symbol = _data.String();

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

        // Act
        var content = new StringContent(symbol, Encoding.UTF8, MediaTypeNames.Text.Plain);
        var response = await client.PostAsync("api/v1/stock/tracked", content);

        // Assert
        response.EnsureSuccessStatusCode();
        var db = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        db.Stock.Should().ContainSingle(
            x => x.UserId == userId && x.Symbol == symbol,
            because: "this is the symbol which the given user has added");
    }

    [Fact]
    public async Task GET_TrackedStocksWithLots_Test()
    {
        // Arrange
        var userId = _data.String();;
        var expectedStocks = Enumerable.Range(0, 10)
            .Select(_ => _data.String())
            .Select(symbol => new TrackedStock
            {
                UserId = userId,
                Symbol = symbol,
                Lots = Enumerable.Range(0, _data.Random.Next(5))
                    .Select(_ => new StockLot()
                    {
                        UserId = userId,
                        Symbol = symbol,
                        Shares = _data.Random.NextDouble(),
                        BuyDate = _data.DateTimeOffset,
                        BuyPrice = _data.Random.NextDouble(),
                        BuyBrokerage = _data.Random.NextDouble(),
                        SoldDate = _data.DateTimeOffset,
                        SoldPrice = _data.Random.NextDouble(),
                        SoldBrokerage = _data.Random.NextDouble(),
                    })
                    .ToList()
            })
            .ToList();

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Stock.AddRange(expectedStocks);
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/stock/tracked");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var stocks = await response.DeserializeContent<List<StockResponse>>();
        stocks.Should().BeEquivalentTo(
            expectedStocks,
            options => options.ExcludingMissingMembers().IncludingNestedObjects(),
            because: "these stocks are stored in the database for the given user");
    }
}
