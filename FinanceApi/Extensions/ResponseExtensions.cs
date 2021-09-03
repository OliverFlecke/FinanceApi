using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinanceApi.Utils
{
    public static class ResponseExtensions
    {
        public static async Task<T?> DeserializeContent<T>(this HttpResponseMessage response) =>
            JsonSerializer.Deserialize<T>(
                await response.Content.ReadAsStringAsync(),
                options: new()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                });
    }
}
