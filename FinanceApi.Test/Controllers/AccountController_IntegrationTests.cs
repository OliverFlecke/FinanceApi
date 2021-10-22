using FinanceApi.Areas.Account.Dtos;
using FinanceApi.Areas.Account.Models;

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
}