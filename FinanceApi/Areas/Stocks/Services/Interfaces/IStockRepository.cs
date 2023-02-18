using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockRepository
{
    IQueryable<TrackedStock> GetTrackedStocksForUser(string userId);

    Task TrackStock(string userId, string symbol);
}
