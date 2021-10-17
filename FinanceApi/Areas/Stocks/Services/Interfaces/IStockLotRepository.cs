using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Models;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockLotRepository
{
    IQueryable<StockLot> GetStockLots(int userId);

    Task AddLot(int userId, AddStockLotRequest request);

    Task UpdateLot(int userId, Guid id, UpdateStockLotRequest request);
}
