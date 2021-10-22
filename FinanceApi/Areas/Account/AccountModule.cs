using FinanceApi.Areas.Account.Services;
using Microsoft.AspNetCore.Routing;

namespace FinanceApi.Areas.Account;

public class AccountModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints) => endpoints;

    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        return services
            .AddTransient<IAccountRepository, AccountRepository>();
    }
}
