using System.Net.Http;

namespace FinanceApi.Utils
{
    public static class ResponseExtensions
    {
        public static async Task<T?> DeserializeContent<T>(this HttpResponseMessage response) =>
            JsonSerializer.Deserialize<T>(
                await response.Content.ReadAsStringAsync(),
                JsonUtils.SerializationOptions);
    }
}
