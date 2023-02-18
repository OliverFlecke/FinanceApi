namespace FinanceApi.Areas.Stocks.Models
{
    public class TrackedStock
    {
#pragma warning disable CS8618 // Consider declaring the property as nullable.
        public string UserId { get; set; }

        public string Symbol { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.

        /// <summary>
        /// Navigational property to linked stock lots.
        /// </summary>
        public IList<StockLot>? Lots { get; set; }
    }
}
