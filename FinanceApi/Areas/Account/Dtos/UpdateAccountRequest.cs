using FinanceApi.Areas.Account.Models;

namespace FinanceApi.Areas.Account.Dtos;

public record UpdateAccountRequest(
    Guid Id,
    string? Name = null,
    AccountType? Type = null,
    string? Currency = null,
    int? SortKey = null);
