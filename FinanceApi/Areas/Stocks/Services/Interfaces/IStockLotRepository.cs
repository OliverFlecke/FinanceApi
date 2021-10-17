using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockLotRepository
{
    Task AddLot(int userId, AddStockLotRequest request);

    Task UpdateLot(int userId, Guid id, UpdateStockLotRequest request);
}
