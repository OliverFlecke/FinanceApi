using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Areas.Stocks.Services;

public interface IStockLotService
{
    Task AddLot(int userId, AddStockLotRequest request);
}
