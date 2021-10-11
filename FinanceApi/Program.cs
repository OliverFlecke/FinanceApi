global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading.Tasks;
global using FinanceApi.Extensions;
using FinanceApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices(builder);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    IdentityModelEventSource.ShowPII = true;
}

app.UseSwagger();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(config =>
{
    app.MapControllers();
});

app.Run();

static void ConfigureServices(WebApplicationBuilder builder)
{
    var services = builder.Services;

    services.AddHttpClient();
    services
        .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie()
        .AddGitHub(options =>
        {
            options.ClientId = builder.Configuration["GitHub:ClientId"];
            options.ClientSecret = builder.Configuration["GitHub:ClientSecret"];
        });

    services
        .AddControllers(options =>
        {
            options.RespectBrowserAcceptHeader = true;
            options.InputFormatters.Add(new RawRequestBodyFormatter());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.AllowTrailingCommas = true;
        });

    services.AddDbContext<FinanceContext>(options =>
    {
        options.UseNpgsql();
    });

    services
        .AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;
        })
        .AddVersionedApiExplorer(options =>
        {
            options.SubstituteApiVersionInUrl = true;
            options.GroupNameFormat = "'v'VVV";
        });
    services
        .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()
        .AddSwaggerGen();

    services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy
                .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
                .AllowCredentials();
        });
    });
}
