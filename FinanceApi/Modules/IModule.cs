using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApi
{
    interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection services) => services;

        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}