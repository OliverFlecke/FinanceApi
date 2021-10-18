using System.Text;
using System.Net.Http;
using FinanceApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using FinanceApi.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FinanceApi.Controllers;

[ApiController]
[ApiVersion("1.0")]
public class AuthenticationController : ControllerBase
{
    const string GITHUB_AUTH_ACCESSTOKEN_URL = "https://github.com/login/oauth/access_token/";

    readonly ILogger<AuthenticationController> _logger;
    readonly IHttpClientFactory _httpClientFactory;
    readonly IConfiguration _configuration;

    public AuthenticationController(
        ILogger<AuthenticationController> logger,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet("/me")]
    [Authorize]
    public ActionResult Get()
    {
        _logger.LogInformation($"Logged in user: '{HttpContext.GetUserId()}'");

        return NoContent();
    }

    [HttpGet("~/signin")]
    public async Task<IActionResult> SignIn(
        [FromQuery] string? provider = "GitHub",
        [FromQuery] string? redirect_uri = "/")
    {
        if (string.IsNullOrWhiteSpace(provider))
        {
            return BadRequest();
        }

        if (!await HttpContext.IsProviderSupportedAsync(provider))
        {
            return BadRequest();
        }

        _logger.LogInformation($"Signing in user through {provider}. Redirect uri: {redirect_uri}");

        return Challenge(new AuthenticationProperties { RedirectUri = redirect_uri }, provider);
    }

    [HttpPost("/signin-github")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<AuthorizeResponse>> Authorize([FromQuery] string code)
    {
        /* Testing this endpoint has to be done with End-to-end testing manually.
            * It requires a user to complete the full loging flow through Github */

        _logger.LogInformation("Authorizing user");

        var client = _httpClientFactory.CreateClient();

        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new(GITHUB_AUTH_ACCESSTOKEN_URL),
        };
        request.Headers.Add("Accept", MediaTypeNames.Application.Json);

        var content = new
        {
            code,
            client_id = _configuration["Github:ClientId"],
            client_secret = _configuration["Github:ClientSecret"],
        };
        request.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json);

        var response = await client.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("User authenticated successfully");
            var auth = await response.DeserializeContent<AuthorizeResponse>();

            return Ok(auth);
        }

        var error = await response.Content.ReadAsStringAsync();
        _logger.LogError($"Failed to authenticate user. Reason: {error}");

        return BadRequest(error);
    }
}
