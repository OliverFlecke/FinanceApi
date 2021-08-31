using System.Net.Http;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using FinanceApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

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

        [HttpPost("~/authorize")]
        public async Task<ActionResult<AuthorizeResponse>> Authorize([FromQuery] string code)
        {
            logger.LogInformation("Authorizing user");

            var client = httpClientFactory.CreateClient();

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new(GITHUB_AUTH_ACCESSTOKEN_URL),
            };
            request.Headers.Add("Content-Type", MediaTypeNames.Application.Json);
            request.Headers.Add("Accept", MediaTypeNames.Application.Json);

            var content = new
            {
                code,
                client_id = configuration["Github:ClientId"],
                client_secret = configuration["Github:ClientSecret"],
            };
            request.Content = new StringContent(JsonSerializer.Serialize(content));

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return Ok(await response.Content.ReadAsStringAsync());
            }

            return BadRequest();
        }
    }
}
