using System.Net.Http;
using System.Text;

namespace FinanceApi.Extensions;

public static class RequestContentUtils
{
    public static StringContent GetJsonContent(object body) =>
        new(JsonSerializer.Serialize(body, JsonUtils.SerializationOptions),
            Encoding.UTF8,
            MediaTypeNames.Application.Json);
}