using System;

namespace FinanceApi.Areas.Stocks.Models
{
    public class StockLot
    {
        public Guid Id { get; set; }

        public int UserId { get; set; }

#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public string Symbol { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.

        public DateTimeOffset BuyDate { get; set; }

        public DateTimeOffset? SoldDate { get; set; }

        public double Shares { get; set; }

        public double Price { get; set; }

        public TrackedStock? TrackedSymbol { get; set; }
    }
}
