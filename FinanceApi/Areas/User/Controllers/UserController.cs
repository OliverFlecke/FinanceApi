// using FinanceApi.Areas.User.Services;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;

// namespace FinanceApi.Areas.User.Controllers;

// [ApiController]
// [ApiVersion("1.0")]
// [Route("api/v{version:apiVersion}/user")]
// public class UserController : ControllerBase
// {
//     [HttpGet("me")]
//     [Authorize]
//     [Produces(MediaTypeNames.Application.Json)]
//     [ProducesResponseType(StatusCodes.Status200OK)]
//     public async Task<ActionResult> GetMe([FromServices] IUserService userService)
//     {
//         return Ok(await userService.GetGithubUser(HttpContext.GetUsername()));
//     }
// }