using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class AuthenticationController : ControllerBase
{
    readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpGet("~/signin")]
    public async Task<IActionResult> SignIn(
        [FromQuery] string? provider = "GitHub",
        [FromQuery] string? returnUrl = "/")
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            return BadRequest();
        }

        if (!await HttpContext.IsProviderSupportedAsync(provider))
        {
            return BadRequest();
        }

        _logger.LogInformation($"Signing in user through {provider}. Redirect uri: {returnUrl}");

        return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, provider);
    }
}
