using System.Net.Http;
using FinanceApi.Areas.Account.Dtos;
using FinanceApi.Areas.Account.Models;
using FinanceApi.Extensions;

namespace FinanceApi.Test.Controllers;

public class AccountController_IntegrationTests
{
    readonly CustomWebApplicationFactory _factory;
    readonly ITestOutputHelper _output;
    readonly DataGenerator _data = new();

    public AccountController_IntegrationTests(ITestOutputHelper output)
    {
        _factory = new(output);
        _output = output;
    }

    [Fact]
    public async Task GET_AccountWithEntries_Test()
    {
        // Arrange
        var userId = _data.Random.Next();
        var dates = Enumerable.Range(0, 10).Select(_ => _data.DateOnly).Distinct().ToList();
        var accounts = new List<Account>()
        {
            new()
            {
                Name = _data.String(),
                Type = AccountType.Cash,

            },
            new()
            {
                Name = _data.String(),
                Type = AccountType.Investment,
            },
            new()
            {
                Name = _data.String(),
                Type = AccountType.Cash,
            },
        };
        foreach (var account in accounts)
        {
            account.UserId = userId;
            account.Entries = dates
                .Select(date => new AccountEntry()
                {
                    Date = date,
                    Amount = _data.Random.NextDouble(),
                })
                .ToList();
        }

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                context.Account.AddRange(accounts);
                await context.SaveChangesAsync();
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        // Act
        var response = await client.GetAsync("api/v1/account");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        _output.WriteLine(await response.Content.ReadAsStringAsync());
        var content = await response.DeserializeContent<List<AccountWithEntriesResponse>>();
        content.Should().BeEquivalentTo(
            accounts,
            options => options.ExcludingMissingMembers().IncludingNestedObjects(),
            because: "these objects has been stored in the database");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("USD")]
    [InlineData("DKK")]
    public async Task POST_AddAccount_Test(string currency)
    {
        // Arrange
        var userId = _data.Random.Next();
        var client = _factory
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var request = new AddAccountRequest(_data.String(), AccountType.Investment, currency);
        var content = RequestContentUtils.GetJsonContent(request);

        // Act
        var response = await client.PostAsync("api/v1/account", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        var account = context.Account.Single(x => x.UserId == userId);
        account.Should().BeEquivalentTo(request, options => options.ExcludingMissingMembers().Excluding(x => x.Currency));

        if (currency is null)
            account.Currency.Should().Be("USD", because: "this is the default currency");
        else
            account.Currency.Should().BeEquivalentTo(currency, because: "this is the requested currency");
    }

    [Fact]
    public async Task POST_AddAccountEntry_Test()
    {
        // Arrange
        var userId = _data.Random.Next();
        Guid? accountId = null;
        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                var entity = context.Account.Add(new()
                {
                    UserId = userId,
                    Name = _data.String(),
                    Type = AccountType.Investment,
                });
                await context.SaveChangesAsync();

                accountId = entity.Entity.Id;
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var request = new AddAccountEntryRequest(accountId!.Value, _data.DateOnly, _data.Random.NextDouble());
        var content = RequestContentUtils.GetJsonContent(request);

        // Act
        var response = await client.PostAsync("api/v1/account/entry", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.AccountEntry.Should().ContainSingle(entry =>
            entry.AccountId == accountId
            && entry.Date == request.Date
            && entry.Amount == request.Amount);
    }

    [Fact]
    public async Task POST_UpdateAccountEntryForExistingEntry_Test()
    {
         // Arrange
        var userId = _data.Random.Next();
        Guid? accountId = null;
        var date = _data.DateOnly;

        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                var entity = context.Account.Add(new()
                {
                    UserId = userId,
                    Name = _data.String(),
                    Type = AccountType.Investment,
                    Entries = new List<AccountEntry>()
                    {
                        new() { Date = date, Amount = _data.Random.NextDouble(), },
                    }
                });
                await context.SaveChangesAsync();

                accountId = entity.Entity.Id;
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var request = new AddAccountEntryRequest(accountId!.Value, date, _data.Random.NextDouble());
        var content = RequestContentUtils.GetJsonContent(request);

        // Act
        var response = await client.PostAsync("api/v1/account/entry", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Accepted);

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.AccountEntry.Include(x => x.Account).Where(x => x.Account!.UserId == userId)
            .Should().ContainSingle(because: "no new entry has been added");

        context.AccountEntry.Should().ContainSingle(entry =>
            entry.AccountId == accountId
            && entry.Date == request.Date
            && entry.Amount == request.Amount);
    }

    [Fact]
    public async Task POST_AddAccountEntryForOtherUser_ShouldNotBeAllowed_Test()
    {

        // Arrange
        var userId = _data.Random.Next();
        Guid? accountId = null;
        var client = _factory
            .SetupDatabase<FinanceContext>(async context =>
            {
                var entity = context.Account.Add(new()
                {
                    UserId = _data.Random.Next(), // Generate other user id
                    Name = _data.String(),
                    Type = AccountType.Investment,
                });
                await context.SaveChangesAsync();

                accountId = entity.Entity.Id;
            })
            .MockAuth(new() { UserId = userId })
            .CreateClient();

        var request = new AddAccountEntryRequest(accountId!.Value, _data.DateOnly, _data.Random.NextDouble());
        var content = RequestContentUtils.GetJsonContent(request);

        // Act
        var response = await client.PostAsync("api/v1/account/entry", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().Be($"Account with id '{request.AccountId}' could not be found");

        var context = _factory.Services.CreateScope().ServiceProvider.GetRequiredService<FinanceContext>();
        context.AccountEntry
            .Include(x => x.Account)
            .Where(x => x.Account!.UserId == userId)
            .Should().BeEmpty();
    }
}