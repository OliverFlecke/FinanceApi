using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace FinanceApi
{
    static class HttpContextExtensions
    {
        public static int GetUserId(this HttpContext context)
        {
            var claim = context.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier);

            return int.Parse(claim.Value);
        }

        public static string? GetUsername(this HttpContext context)
        {
            return context.User.Identity?.Name;
        }

        public static async Task<AuthenticationScheme[]> GetExternalProvidersAsync(this HttpContext context)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            var schemes = context.RequestServices.GetRequiredService<IAuthenticationSchemeProvider>();

            return (from scheme in await schemes.GetAllSchemesAsync()
                    where !string.IsNullOrEmpty(scheme.DisplayName)
                    select scheme).ToArray();
        }

        public static async Task<bool> IsProviderSupportedAsync(this HttpContext context, string provider)
        {
            if (context is null) throw new ArgumentNullException(nameof(context));

            return (from scheme in await context.GetExternalProvidersAsync()
                    where string.Equals(scheme.Name, provider, StringComparison.OrdinalIgnoreCase)
                    select scheme).Any();
        }
    }
}
