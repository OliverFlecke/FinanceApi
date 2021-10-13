using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
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

        static IActionResult GetHealth() => new OkResult();
    }
}