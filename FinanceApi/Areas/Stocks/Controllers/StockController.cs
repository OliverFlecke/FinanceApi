using System.Net.Mime;
using System.Collections.Generic;
using FinanceApi.Areas.Stocks.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Areas.Stocks.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/stock")]
    public class StockController : ControllerBase
    {
        [HttpGet]
        [Produces(MediaTypeNames.Application.Json)]
        public ActionResult<IList<StockDto>> GetSymbol()
        {
            return Ok(new List<StockDto>());
        }
    }
}
