using FinanceApi;
using FinanceApi.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto,
});

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
        .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            var config = builder.Configuration
                .GetSection(Auth0Options.SectionName)
                .Get<Auth0Options>()!;

            options.Authority = $"https://{config.Domain}/";
            options.Audience = config.Audience;
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

            foreach (var converter in JsonUtils.SerializationOptions.Converters)
            {
                options.JsonSerializerOptions.Converters.Add(converter);
            }
        });

    services.AddDbContext<FinanceContext>(options =>
    {
        var config = builder.Configuration
            .GetSection(DbOptions.SectionName)
            .Get<DbOptions>()!;
        options.UseNpgsql(config.ConnectionString);
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
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
    });
}

#pragma warning disable CA1050
public partial class Program { }
#pragma warning restore CA1050