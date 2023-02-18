using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Services;

class StockRepository : IStockRepository
{
    readonly ILogger<StockRepository> _logger;
    readonly FinanceContext _context;

    public StockRepository(
        ILogger<StockRepository> logger,
        FinanceContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IQueryable<TrackedStock> GetTrackedStocksForUser(string userId)
    {
        _logger.LogInformation($"Getting tracked stocks for user '{userId}'");

        return _context.Stock.Where(x => x.UserId == userId);
    }

    public async Task TrackStock(string userId, string symbol)
    {
        _logger.LogInformation($"Start tracking '{symbol}' for user '{userId}'");

        var stock = await _context.Stock.FindAsync(userId, symbol);
        if (stock is null)
        {
            _context.Stock.Add(new()
            {
                UserId = userId,
                Symbol = symbol,
            });
            await _context.SaveChangesAsync();
        }
    }
}