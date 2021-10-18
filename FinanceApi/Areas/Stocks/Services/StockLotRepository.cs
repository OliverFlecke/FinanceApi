using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Services;

class StockLotRepository : IStockLotRepository
{
    readonly ILogger<StockLotRepository> _logger;
    readonly FinanceContext _context;
    private readonly IStockRepository _stockRepository;

    public StockLotRepository(
        ILogger<StockLotRepository> logger,
        FinanceContext context,
        IStockRepository stockRepository)
    {
        _logger = logger;
        _context = context;
        _stockRepository = stockRepository;
    }

    /// <inheritdoc />
    public IQueryable<StockLot> GetStockLots(int userId)
    {
        return _context.StockLot.Where(x => x.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task<Guid> AddLot(int userId, AddStockLotRequest request)
    {
        _logger.LogInformation($"Addding stock lot for user '{userId}':\n {request}");

        await _stockRepository.TrackStock(userId, request.Symbol);

        var lot = _context.StockLot.Add(new() {
            UserId = userId,
            Symbol = request.Symbol,
            Shares = request.Shares,
            BuyDate = request.BuyDate,
            BuyPrice = request.BuyPrice,
            BuyBrokerage = request.BuyBrokerage,
            SoldDate = request.SoldDate,
            SoldPrice = request.SoldPrice,
            SoldBrokerage = request.SoldBrokerage,
        });
        await _context.SaveChangesAsync();

        return lot.Entity.Id;
    }

    /// <inheritdoc/>
    public async Task UpdateLot(int userId, Guid id, UpdateStockLotRequest request)
    {
        var entity = await _context.StockLot.FindAsync(id);
        if (entity is null || entity.UserId != userId) throw new EntityNotFoundException($"Lot with id '{id}' was not found");

        entity.Shares = request.Shares ?? entity.Shares;
        entity.BuyDate = request.BuyDate ?? entity.BuyDate;
        entity.BuyPrice = request.BuyPrice ?? entity.BuyPrice;
        entity.BuyBrokerage = request.BuyBrokerage ?? entity.BuyBrokerage;
        entity.SoldDate = request.SoldDate ?? entity.SoldDate;
        entity.SoldPrice = request.SoldPrice ?? entity.SoldPrice;
        entity.SoldBrokerage = request.SoldBrokerage ?? entity.SoldBrokerage;

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteLot(int userId, Guid lotId)
    {
        var entity = await _context.StockLot.FindAsync(lotId);
        if (entity is null || entity.UserId != userId) throw new EntityNotFoundException($"Lot with id '{lotId}' was not found");

        _context.StockLot.Remove(entity);

        await _context.SaveChangesAsync();
    }
}