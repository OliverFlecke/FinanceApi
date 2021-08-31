namespace FinanceApi.Areas.Stocks.Models
{
    public class TrackedStock
    {
        public int UserId { get; set; }

#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public string Symbol { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.
    }
}
