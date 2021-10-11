using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FinanceApi.Test.Utils
{
    public static class WebApplicationFactoryExtensions
    {
        public static WebApplicationFactory<TEntryPoint> SetupDatabase<TContext, TEntryPoint>(
            this WebApplicationFactory<TEntryPoint> factory,
            Func<TContext, Task> configure)
            where TEntryPoint : class
            where TContext : DbContext
        {
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            configure(context);

            return factory;
        }

        public static WebApplicationFactory<Program> SetupDatabase<TContext>(
            this WebApplicationFactory<Program> factory,
            Func<TContext, Task> configure)
            where TContext : DbContext
            => SetupDatabase<TContext, Program>(factory, configure);
    }
}
