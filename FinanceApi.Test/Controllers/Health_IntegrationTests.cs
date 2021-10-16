namespace FinanceApi.Test
{
    public class Health_IntegrationTests
    {
        readonly CustomWebApplicationFactory _factory = new();

        public Health_IntegrationTests(ITestOutputHelper output)
        {
            _factory = new(output);
        }

        [Fact(Skip = "Issue with endpoint being registered multiple times during test")]
        public async Task GET_HealthEndpoint()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/");

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
