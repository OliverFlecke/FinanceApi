using FinanceApi.Areas.Account.Models;

namespace FinanceApi.Areas.Account.Dtos;

public record AddAccountRequest(string Name, AccountType Type, string? Currency = null);
