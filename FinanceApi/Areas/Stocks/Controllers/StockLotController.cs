using System.Net.Mime;
using FinanceApi.Areas.Stocks.Dtos;
using FinanceApi.Areas.Stocks.Extensions;
using FinanceApi.Areas.Stocks.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Areas.Stocks.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/stock/lot")]
public class StockLotController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public ActionResult<IList<StockLotResponse>> GetStockLots([FromServices] IStockLotRepository repository)
    {
        return Ok(repository
            .GetStockLots(HttpContext.GetUserId())
            .Select(lot => lot.ToStockLotResponse()));
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<Guid>> Post(
        [FromBody] AddStockLotRequest request,
        [FromServices] IStockLotRepository service)
    {
        return Ok(await service.AddLot(HttpContext.GetUserId(), request));
    }

    [HttpPut("{lotId}")]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult> Update(
        [FromRoute] Guid lotId,
        [FromBody] UpdateStockLotRequest request,
        [FromServices] IStockLotRepository service)
    {
        await service.UpdateLot(HttpContext.GetUserId(), lotId, request);

        return Accepted();
    }
}