using FinanceApi.Areas.Account.Dtos;

namespace FinanceApi.Areas.Account.Extensions;

static class AccountExtensions
{
    public static AccountWithEntriesResponse ToAccountWithEntriesResponse(this Models.Account account) =>
        new(
            account.Id,
            account.Name,
            account.Type,
            account.Entries?.Select(e => e.ToEntryResponse()));
}