using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using FinanceApi.Areas.Stocks.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Modules.Stocks.Endpoints;

class GetStockSymbol
{
    const string YahooBaseUrl = "https://query2.finance.yahoo.com/v7/finance/quote";

    static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    internal static async Task<IResult> GetSymbol(
        [FromQuery] QueryParameters symbols,
        [FromServices] ILogger<GetStockSymbol> logger,
        [FromServices] IHttpClientFactory clientFactory)
    {
        // if (symbols is null || symbols.Count == 0) return Results.BadRequest("");

        logger.LogInformation($"Handling request for symbols: {string.Join(", ", symbols)}");

        var client = clientFactory.CreateClient();

        var uri = new UriBuilder(YahooBaseUrl)
        {
            Query = $"symbols={string.Join(",", symbols)}",
        };
        var response = await client.GetAsync(uri.ToString());
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var yahooResponse = JsonSerializer.Deserialize<YahooResponse>(content, options: _options);

            if (yahooResponse?.QuoteResponse?.Error is not null) return Results.BadRequest(yahooResponse.QuoteResponse.Error);
            if (yahooResponse?.QuoteResponse is null) return Results.BadRequest("Unable to retreive quotes for given symbols");

            return Results.Ok(yahooResponse.QuoteResponse.Result);
        }

        var errorResponse = JsonSerializer.Deserialize<YahooFinanceResponse>(content, options: _options);

        return Results.BadRequest(errorResponse?.Finance?.Error);
    }
}