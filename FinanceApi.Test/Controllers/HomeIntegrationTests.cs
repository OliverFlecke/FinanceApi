namespace FinanceApi.Test
{
    public class HomeIntegrationTests
    {
        readonly CustomWebApplicationFactory _factory = new();

        [Fact]
        public async Task GET_HomeEndpoint()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/api/v1/");

            // Assert
            response.EnsureSuccessStatusCode();
            (await response.Content.ReadAsStringAsync()).Should().BeEmpty();
        }
    }
}
