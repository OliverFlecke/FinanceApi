using System.Text.Json.Serialization;
using FinanceApi.Converters;

namespace FinanceApi.Extensions;

public static class JsonUtils
{
    public static readonly JsonSerializerOptions SerializationOptions;

    static JsonUtils()
    {
        SerializationOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };
        SerializationOptions.Converters.Add(new DateOnlyJsonConverter());
        SerializationOptions.Converters.Add(new JsonStringEnumConverter());
    }
}