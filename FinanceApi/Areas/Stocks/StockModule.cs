using FinanceApi.Areas.Stocks.Services;
using Microsoft.AspNetCore.Routing;

namespace FinanceApi.Areas.Stocks;

public class StockModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services
            .AddTransient<IStockRepository, StockRepository>()
            .AddTransient<IStockLotService, StockLotService>();

        return services;
    }

    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;
}