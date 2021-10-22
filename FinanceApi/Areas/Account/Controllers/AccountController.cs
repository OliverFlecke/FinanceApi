using FinanceApi.Areas.Account.Dtos;
using FinanceApi.Areas.Account.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Areas.Account.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/account")]
public class AccountController : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<AccountWithEntriesResponse>> Get(
        [FromServices] IAccountRepository accountRepository)
    {
        return Ok(await accountRepository.GetAccountsWithEntries(HttpContext.GetUserId()));
    }

    [HttpPost]
    [Authorize]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    public async Task<ActionResult<Guid>> Post(
        [FromBody] AddAccountRequest request,
        [FromServices] IAccountRepository accountRepository)
    {
        return Accepted(await accountRepository.AddAccount(HttpContext.GetUserId(), request.Name, request.Type));
    }
}