using System;
using System.Net.Mime;
using System.Collections.Generic;
using FinanceApi.Areas.Stocks.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace FinanceApi.Areas.Stocks.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stock")]
    public class StockController : ControllerBase
    {
        const string YahooBaseUrl = "https://query2.finance.yahoo.com/v7/finance/quote";

        readonly ILogger<StockController> logger;
        readonly IHttpClientFactory clientFactory;

        public StockController(
            ILogger<StockController> logger,
            IHttpClientFactory clientFactory)
        {
            this.logger = logger;
            this.clientFactory = clientFactory;
        }

        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public async Task<ActionResult<IList<StockDto>>> GetSymbol([FromQuery] IList<string> symbols)
        {
            this.logger.LogInformation($"Handling request for symbols: {string.Join(", ", symbols)}");

            var client = this.clientFactory.CreateClient();

            var uri = new UriBuilder(YahooBaseUrl)
            {
                Query = $"symbols={string.Join(",", symbols)}",
            };
            var response = await client.GetAsync(uri.ToString());

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var yahooResponse = JsonSerializer.Deserialize<YahooResponse>(content, options: new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });

                if (yahooResponse?.QuoteResponse is null) return BadRequest("Unable to retreive quotes for given symbols");
                if (yahooResponse.QuoteResponse.Error is not null) return BadRequest(yahooResponse.QuoteResponse.Error);

                return Ok(yahooResponse.QuoteResponse.Result);
            }

            return BadRequest();
        }
    }

    public class YahooResponse
    {
        public QuoteResponse? QuoteResponse { get; set; }
    }

    public class QuoteResponse
    {
        public List<StockDto>? Result { get; set; }

        public string? Error { get; set; }
    }
}
