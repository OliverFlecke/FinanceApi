namespace FinanceApi.Areas.Account.Dtos;

public record AddAccountEntryRequest(Guid AccountId, DateOnly Date, double Amount);