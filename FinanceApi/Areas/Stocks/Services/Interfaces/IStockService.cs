using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockService
{
    Task<IList<StockResponse>> GetSymbols(IEnumerable<string> symbols);
}
