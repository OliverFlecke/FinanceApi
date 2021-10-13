using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FinanceApi.Endpoints
{
    public class HealthModule : IModule
    {
        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/api/v1/", GetHealth);

            return endpoints;
        }

        static IResult GetHealth() => Results.Ok();
    }
}