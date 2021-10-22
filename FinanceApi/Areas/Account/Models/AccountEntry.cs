namespace FinanceApi.Areas.Account.Models;

public class AccountEntry
{
    public DateOnly Date { get; set; }

    public double Amount { get; set; }

    public Guid AccountId { get; set; }

    public Account? Account { get; set; }
}