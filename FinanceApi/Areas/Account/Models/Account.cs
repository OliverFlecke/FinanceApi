namespace FinanceApi.Areas.Account.Models;

public class Account
{
    public Guid Id { get; set; }

#pragma warning disable CS8618 // Consider declaring the property as nullable.
    public string UserId { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.

    public AccountType Type { get; set; }

#pragma warning disable CS8618 // Consider declaring the property as nullable.
    public string Name { get; set; }

    public string Currency { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.

    public int SortKey { get; set; }

    public List<AccountEntry>? Entries { get; set; }
}