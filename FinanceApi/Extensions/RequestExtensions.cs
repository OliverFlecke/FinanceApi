using System.Net.Http;
using System.Text;

namespace FinanceApi.Extensions;

public static class RequestContentUtils
{
    public static StringContent GetJsonContent(object body)
    {
        return new StringContent(JsonSerializer.Serialize(body), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}