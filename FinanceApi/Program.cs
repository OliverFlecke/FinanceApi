global using System;
global using System.Collections.Generic;
global using System.Linq;
global using System.Text.Json;
global using System.Threading.Tasks;
global using FinanceApi.Exceptions;
global using FinanceApi.Extensions;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;
global using MediaTypeNames = System.Net.Mime.MediaTypeNames;
using FinanceApi;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder);
builder.RegisterModules();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    IdentityModelEventSource.ShowPII = true;
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapEndpoints();

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
            options.Filters.Add(new HttpResponseExceptionFilter());
        })
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.AllowTrailingCommas = true;
        });

    services.AddDbContext<FinanceContext>(options =>
    {
        var config = builder.Configuration;
        options.UseNpgsql($"host={config["DB:Host"]};database={config["DB:Database"]};user id={config["DB:User"]};password={config["DB:Password"]}");
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
        .AddEndpointsApiExplorer()
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

public partial class Program { }