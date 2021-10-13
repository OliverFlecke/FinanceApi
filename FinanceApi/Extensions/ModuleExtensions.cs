using Microsoft.AspNetCore.Builder;


namespace FinanceApi.Extensions
{
    static class ModuleExtensions
    {
        static bool _hasBeenRegistered;

        static readonly List<IModule> registeredModules = new();

        public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
        {
            if (_hasBeenRegistered) return builder;
            _hasBeenRegistered = true;

            foreach (var module in DiscoverModules())
            {
                module.RegisterModule(builder.Services);
                registeredModules.Add(module);
            }

            return builder;
        }

        public static WebApplication MapEndpoints(this WebApplication app)
        {
            foreach (var module in registeredModules)
            {
                module.MapEndpoints(app);
            }

            return app;
        }

        static IEnumerable<IModule> DiscoverModules() =>
            typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}