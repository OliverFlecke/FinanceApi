using System.Net.Mime;
using System.Net.Http;
using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinanceApi.Areas.Stocks.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/stock/lot")]
public class StockLotController : ControllerBase
{
    readonly ILogger<StockLotController> _logger;

    public StockLotController(ILogger<StockLotController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult> Post(
        [FromBody] AddStockLotRequest request,
        [FromServices] IStockLotService service)
    {
        await service.AddLot(HttpContext.GetUserId(), request);

        return Accepted();
    }
}