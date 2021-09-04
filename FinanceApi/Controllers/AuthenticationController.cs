using System.Text;
using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using FinanceApi.Utils;
using Microsoft.AspNetCore.Authentication;

namespace FinanceApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    public class AuthenticationController : ControllerBase
    {
        const string GITHUB_AUTH_ACCESSTOKEN_URL = "https://github.com/login/oauth/access_token/";

        readonly ILogger<AuthenticationController> logger;
        readonly IHttpClientFactory httpClientFactory;
        readonly IConfiguration configuration;

        public AuthenticationController(
            ILogger<AuthenticationController> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.configuration = configuration;
        }

        [HttpPost("/authorize")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<AuthorizeResponse>> Authorize([FromQuery] string code)
        {
            /* Testing this endpoint has to be done with End-to-end testing manually.
             * It requires a user to complete the full loging flow through Github */

            logger.LogInformation("Authorizing user");

            var client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new(GITHUB_AUTH_ACCESSTOKEN_URL),
            };
            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var content = new
            {
                code,
                client_id = configuration["Github:ClientId"],
                client_secret = configuration["Github:ClientSecret"],
            };
            request.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json);

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation("User authenticated successfully");
                var auth = await response.DeserializeContent<AuthorizeResponse>();

                return Ok(auth);
            }

            var error = await response.Content.ReadAsStringAsync();
            logger.LogError($"Failed to authenticate user. Reason: {error}");

            return BadRequest(error);
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

            return Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, provider);
        }
    }
}
