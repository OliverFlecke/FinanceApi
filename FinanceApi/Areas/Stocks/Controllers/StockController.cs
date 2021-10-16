using FinanceApi.Areas.Stocks.Dtos;
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
        readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger) => _logger = logger;

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
        public ActionResult<IList<StockResponse>> GetTracked([FromServices] IStockRepository service)
        {
            return Ok(service
                .GetTrackedStocksForUser(HttpContext.GetUserId())
                .Select(x => new StockResponse { Symbol = x.Symbol }));
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
