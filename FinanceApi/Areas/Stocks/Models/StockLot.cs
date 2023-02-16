namespace FinanceApi.Areas.Stocks.Models
{
    public class StockLot
    {
        public Guid Id { get; set; }

#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public string UserId { get; set; }

        public string Symbol { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.

        public double Shares { get; set; }

        public DateTimeOffset BuyDate { get; set; }

        public double BuyPrice { get; set; }

        public double BuyBrokerage { get; set; }

        public DateTimeOffset? SoldDate { get; set; }

        public double? SoldPrice { get; set; }

        public double? SoldBrokerage { get; set; }

        public TrackedStock? TrackedSymbol { get; set; }
    }
}
