using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace FinanceApi.Test.Utils
{
    class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        readonly TestAuthData? _data;

        public TestAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            TestAuthData? data)
            : base(options, logger, encoder, clock)
        {
            _data = data;
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>();
            if (_data is not null)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, _data.UserId.ToString()));

                if (!string.IsNullOrWhiteSpace(_data.Name)) claims.Add(new Claim(ClaimTypes.Name, _data.Name));
            }
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }

    static class TestAuthHandlerExtensions
    {
        public static WebApplicationFactory<T> MockAuth<T>(
            this WebApplicationFactory<T> factory,
            TestAuthData? authData = null)
            where T : class
        {
            return factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    services.AddAuthentication("Test")
                        .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                            "Test", options => { });

                    if (authData is not null)
                    {
                        services.AddSingleton<TestAuthData>(authData);
                    }
                });
            });
        }
    }

    class TestAuthData
    {
        public int UserId { get; set; }

        public string? Name { get; set; }
    }
}
