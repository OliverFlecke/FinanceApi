using System.Net.Http;
using FinanceApi.Areas.Stocks.Dtos;

namespace FinanceApi.Areas.Stocks.Services;

class StockService : IStockService
{
    readonly ILogger<StockService> _logger;
    readonly IHttpClientFactory _clientFactory;

    const string YahooBaseUrl = "https://query2.finance.yahoo.com/v7/finance/quote";

    static readonly JsonSerializerOptions _options = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    public StockService(
        ILogger<StockService> logger,
        IHttpClientFactory clientFactory)
    {
        _logger = logger;
        _clientFactory = clientFactory;
    }

    public async Task<IList<StockResponse>> GetSymbols(IEnumerable<string> symbols)
    {
        _logger.LogInformation($"Handling request for symbols: {string.Join(", ", symbols)}");

        var client = _clientFactory.CreateClient();

        var uri = new UriBuilder(YahooBaseUrl)
        {
            Query = $"symbols={string.Join(",", symbols)}",
        };
        var response = await client.GetAsync(uri.ToString());
        var content = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            var yahooResponse = JsonSerializer.Deserialize<YahooResponse>(content, options: _options);

            if (yahooResponse?.QuoteResponse?.Error is not null) throw new Exception(yahooResponse.QuoteResponse.Error.Description);
            if (yahooResponse?.QuoteResponse is null) throw new Exception("Unable to retreive quotes for given symbols");

            return yahooResponse?.QuoteResponse?.Result ?? new List<StockResponse>();
        }

        var errorResponse = JsonSerializer.Deserialize<YahooFinanceResponse>(content, options: _options);

        throw new Exception(errorResponse?.Finance?.Error?.Description);
    }
}