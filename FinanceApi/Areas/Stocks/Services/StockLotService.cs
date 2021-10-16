using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Areas.Stocks.Services;

public class StockLotService : IStockLotService
{
    readonly ILogger<StockLotService> _logger;
    readonly FinanceContext _context;

    public StockLotService(
        ILogger<StockLotService> logger,
        FinanceContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task AddLot(int userId, AddStockLotRequest request)
    {
        _logger.LogInformation($"Addding stock lot for user '{userId}':\n {request}");

        var stock = await _context.Stock.FindAsync(userId, request.Symbol);
        if (stock is null)
        {
            _context.Stock.Add(new()
            {
                UserId = userId,
                Symbol = request.Symbol,
            });
        }

        _context.StockLot.Add(new() {
            UserId = userId,
            Symbol = request.Symbol,
            BuyDate = request.BuyDate,
            Shares = request.Shares,
            Price = request.Price,
        });
        await _context.SaveChangesAsync();
    }
}