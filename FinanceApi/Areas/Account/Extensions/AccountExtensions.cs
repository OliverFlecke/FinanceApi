using FinanceApi.Areas.Account.Dtos;

namespace FinanceApi.Areas.Account.Extensions;

static class AccountExtensions
{
    public static AccountWithEntriesResponse ToAccountWithEntriesResponse(this Models.Account account) =>
        new(
            account.Id,
            account.Name,
            account.Type,
            account.Currency,
            account.SortKey,
            account.Entries?.Select(e => e.ToEntryResponse()));

    public static Models.Account FromUpdateAccountRequest(this UpdateAccountRequest account) =>
        new()
        {
            Id = account.Id,
            Type = account.Type ?? default,
            // We allow null to be assigned here, as these fields are only read when modified.
            Name = account.Name ?? default!,
            Currency = account.Currency ?? default!,
            SortKey = account.SortKey ?? default,
        };
}