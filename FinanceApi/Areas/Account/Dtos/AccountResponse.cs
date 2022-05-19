using FinanceApi.Areas.Account.Models;

namespace FinanceApi.Areas.Account.Dtos;

public record AccountResponse(
    Guid Id,
    string Name,
    AccountType Type,
    string Currency,
    int SortKey);
