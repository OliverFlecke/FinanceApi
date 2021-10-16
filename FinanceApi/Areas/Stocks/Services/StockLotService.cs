using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Areas.Stocks.Services;

public class StockLotService : IStockLotService
{
    readonly ILogger<StockLotService> _logger;
    readonly FinanceContext _context;
    private readonly IStockRepository _stockRepository;

    public StockLotService(
        ILogger<StockLotService> logger,
        FinanceContext context,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _context = context;
        _stockRepository = stockRepository;
    }

    public async Task AddLot(int userId, AddStockLotRequest request)
    {
        _logger.LogInformation($"Addding stock lot for user '{userId}':\n {request}");

        await _stockRepository.TrackStock(userId, request.Symbol);

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