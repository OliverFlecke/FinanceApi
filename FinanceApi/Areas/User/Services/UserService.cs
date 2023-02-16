// using System.Net.Http;
// using System.Net.Http.Headers;
// using FinanceApi.Areas.User.Dtos;
// using FinanceApi.Options;
// using FinanceApi.Utils;
// using Microsoft.Extensions.Options;

// namespace FinanceApi.Areas.User.Services;

// class UserService : IUserService
// {
//     const string GithubApiUri = "https://api.github.com";

//     readonly ILogger<UserService> _logger;
//     readonly IHttpClientFactory _httpClientFactory;
//     readonly IOptions<GithubOptions> _githubOptions;

//     public UserService(
//         ILogger<UserService> logger,
//         IHttpClientFactory httpClientFactory,
//         IOptions<GithubOptions> githubOptions)
//     {
//         _logger = logger;
//         _httpClientFactory = httpClientFactory;
//         _githubOptions = githubOptions;
//     }

//     /// <inheritdoc/>
//     public async Task<UserResponse?> GetGithubUser(string? username)
//     {
//         if (username is null)
//         {
//             _logger.LogWarning($"No GitHub username was provided");
//             return null;
//         }

//         _logger.LogInformation($"Getting Github user '{username}'");

//         var client = _httpClientFactory.CreateClient("github");

//         var request = new HttpRequestMessage(HttpMethod.Get, $"{GithubApiUri}/users/{username}");
//         request.Headers.Authorization = new AuthenticationHeaderValue("Basic", $"{_githubOptions.Value.ClientId}:{_githubOptions.Value.ClientSecret}");
//         request.Headers.Add("User-Agent", "finance-tools");

//         var response = await client.SendAsync(request);

//         return await response.DeserializeContent<UserResponse>();
//     }
// }