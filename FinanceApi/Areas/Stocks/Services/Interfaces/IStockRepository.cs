using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockRepository
{
    IQueryable<TrackedStock> GetTrackedStocksForUser(int userId);

    Task TrackStock(int userId, string symbol);
}
