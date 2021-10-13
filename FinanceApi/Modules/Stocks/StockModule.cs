using FinanceApi.Modules.Stocks.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace FinanceApi.Modules.Stocks;

class StockModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // endpoints.MapGet("/api/v1/stock", GetStockSymbol.GetSymbol)
        //     .WithGroupName("v1");

        return endpoints;
    }
}