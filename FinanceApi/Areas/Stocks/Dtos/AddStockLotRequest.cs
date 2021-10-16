namespace FinanceApi.Areas.Stocks.Dtos;

public record AddStockLotRequest
{
#pragma warning disable CS8618 // Consider declaring the property as nullable.
    public string Symbol { get; set; }
#pragma warning restore CS8618 // Consider declaring the property as nullable.

    public DateTimeOffset BuyDate { get; set; }

    public DateTimeOffset? SoldDate { get; set; }

    public double Shares { get; set; }

    public double Price { get; set; }
}