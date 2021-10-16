namespace FinanceApi.Areas.Stocks.Dtos;

public record UpdateStockLotRequest
{
    public DateTimeOffset? BuyDate { get; set; }

    public DateTimeOffset? SoldDate { get; set; }

    public double? Shares { get; set; }

    public double? Price { get; set; }
}