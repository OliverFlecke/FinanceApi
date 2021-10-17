namespace FinanceApi.Areas.Stocks.Dtos;

public record UpdateStockLotRequest
{
    public double? Shares { get; set; }

    public DateTimeOffset? BuyDate { get; set; }

    public double? BuyPrice { get; set; }

    public double? BuyBrokerage { get; set; }

    public DateTimeOffset? SoldDate { get; set; }

    public double? SoldPrice { get; set; }

    public double? SoldBrokerage { get; set; }
}