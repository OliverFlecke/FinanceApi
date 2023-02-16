using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockLotRepository
{
    IQueryable<StockLot> GetStockLots(string userId);

    Task<Guid> AddLot(string userId, AddStockLotRequest request);

    Task UpdateLot(string userId, Guid id, UpdateStockLotRequest request);

    Task DeleteLot(string userId, Guid lotId);
}
