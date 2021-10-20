using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Extensions;
using FinanceApi.Areas.Stocks.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Areas.Stocks.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stock")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IList<StockResponse>>> GetSymbol(
            [FromQuery] IList<string> symbols,
            [FromServices] IStockService stockService)
        {
            try
            {
                return Ok(await stockService.GetSymbols(symbols));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("tracked")]
        [Authorize]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IList<StockResponse>>> GetTracked([FromServices] IStockRepository service)
        {
            var stocks = await service
                .GetTrackedStocksForUser(HttpContext.GetUserId())
                .Include(s => s.Lots)
                .ToListAsync();

            return Ok(stocks.Select(x => new StockResponse
                {
                    Symbol = x.Symbol,
                    Lots = x.Lots?.Select(lot => lot.ToStockLotResponse()).ToList() ?? new List<StockLotResponse>(),
                }));
        }

        [HttpPost("tracked")]
        [Authorize]
        [Consumes(MediaTypeNames.Text.Plain, MediaTypeNames.Application.Json)]
        public async Task<ActionResult> AddTrackedStock(
            [FromBody] string symbol,
            [FromServices] IStockRepository service)
        {
            await service.TrackStock(HttpContext.GetUserId(), symbol);

            return Ok();
        }
    }
}
