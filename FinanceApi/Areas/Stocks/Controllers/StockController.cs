using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceApi.Areas.Stocks.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FinanceApi.Areas.Stocks.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stock")]
    public class StockController : ControllerBase
    {
        const string YahooBaseUrl = "https://query2.finance.yahoo.com/v7/finance/quote";

        static readonly JsonSerializerOptions _options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<StockDto>>> GetSymbol([FromQuery] IList<string> symbols)
        {
            this.logger.LogInformation($"Handling request for symbols: {string.Join(", ", symbols)}");

            var client = this.clientFactory.CreateClient();

            var uri = new UriBuilder(YahooBaseUrl)
            {
                Query = $"symbols={string.Join(",", symbols)}",
            };
            var response = await client.GetAsync(uri.ToString());
            var content = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var yahooResponse = JsonSerializer.Deserialize<YahooResponse>(content, options: _options);

                if (yahooResponse?.QuoteResponse?.Error is not null) return BadRequest(yahooResponse.QuoteResponse.Error);
                if (yahooResponse?.QuoteResponse is null) return BadRequest("Unable to retreive quotes for given symbols");

                return Ok(yahooResponse.QuoteResponse.Result);
            }

            var errorResponse = JsonSerializer.Deserialize<YahooFinanceResponse>(content, options: _options);

            return BadRequest(errorResponse?.Finance?.Error);
        }
    }
}
