using System;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace FinanceApi.Test
{
    public class HomeIntegrationTests : IClassFixture<CustomWebApplicationFactory>
    {
        readonly CustomWebApplicationFactory _factory;

        public HomeIntegrationTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetHomeEndpoint()
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
