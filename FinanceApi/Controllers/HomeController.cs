using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAsync() => Ok();
    }
}