using System.Net.Mime;
using System.Net.Http;
using FinanceApi.Areas.Stocks.Dtos;
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
        [FromServices] FinanceContext context)
    {
        var userId = HttpContext.GetUserId();
        _logger.LogInformation($"Added stock lot for user '{userId}':\n {request}");

        context.StockLot.Add(new() {
            UserId = userId,
            Symbol = request.Symbol,
            BuyDate = request.BuyDate,
            Shares = request.Shares,
            Price = request.Price,
        });
        await context.SaveChangesAsync();

        return Accepted();
    }
}