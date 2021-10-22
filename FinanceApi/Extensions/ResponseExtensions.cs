using System.Net.Http;
using FinanceApi.Converters;

namespace FinanceApi.Utils
{
    public static class ResponseExtensions
    {
        static readonly JsonSerializerOptions _options;

        static ResponseExtensions()
        {
            _options = new()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };
            _options.Converters.Add(new DateOnlyJsonConverter());
        }

        public static async Task<T?> DeserializeContent<T>(this HttpResponseMessage response) =>
            JsonSerializer.Deserialize<T>(
                await response.Content.ReadAsStringAsync(),
                _options);
    }
}
