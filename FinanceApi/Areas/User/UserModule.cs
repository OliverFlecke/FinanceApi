using FinanceApi.Areas.User.Services;
using Microsoft.AspNetCore.Routing;

namespace FinanceApi.Areas.User;

class UserModule : IModule
{
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        return endpoints;
    }

    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        return services;
            // .AddTransient<IUserService, UserService>();
    }
}
