using FinanceApi.Areas.Account.Models;

namespace FinanceApi.Areas.Account.Dtos;

static class AccountEntryExtensions
{
    public static EntryResponse ToEntryResponse(this AccountEntry entry) =>
        new(entry.Date, entry.Amount);
}