using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Extensions;

static class StockLotExtensions
{
    public static StockLotResponse ToStockLotResponse(this StockLot lot) => new()
    {
        Id = lot.Id,
        Symbol = lot.Symbol,
        Shares = lot.Shares,
        BuyDate = lot.BuyDate,
        BuyPrice = lot.BuyPrice,
        BuyBrokerage = lot.BuyBrokerage,
        SoldDate = lot.SoldDate,
        SoldPrice = lot.SoldPrice,
        SoldBrokerage = lot.SoldBrokerage,
    };
}