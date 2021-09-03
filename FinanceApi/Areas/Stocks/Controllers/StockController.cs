using System.Net;
using System.Security.Claims;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceApi.Areas.Stocks.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

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
        readonly FinanceContext context;

        public StockController(
            ILogger<StockController> logger,
            IHttpClientFactory clientFactory,
            FinanceContext context)
        {
            this.logger = logger;
            this.clientFactory = clientFactory;
            this.context = context;
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


        [HttpGet("tracked")]
        [Authorize]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IList<StockDto>>> GetTracked()
        {
            var userId = HttpContext.GetUserId();
            this.logger.LogInformation($"Getting tracked stocks for {userId}");

            var stocks = await this.context.Stock
                .Where(x => x.UserId == userId)
                .ToListAsync();
            var dtos = stocks.Select(x => new StockDto
            {
                Symbol = x.Symbol,
            });

            return Ok(dtos);
        }

        [HttpPost("tracked")]
        [Authorize]
        [Consumes(MediaTypeNames.Text.Plain, MediaTypeNames.Application.Json)]
        public async Task<ActionResult> AddTrackedStock([FromBody] string symbol)
        {
            var userId = HttpContext.GetUserId();
            logger.LogInformation($"Adding symbol '{symbol}' for user {userId}");

            context.Stock.Add(new()
            {
                UserId = userId,
                Symbol = symbol,
            });
            await context.SaveChangesAsync();

            return Ok();
        }
    }
}
