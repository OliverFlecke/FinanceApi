namespace FinanceApi.Extensions;

public class QueryParameters : List<string>
{
    public static bool TryParse(string value, out QueryParameters? result)
    {
        if (value is not null)
        {
            result = new();
            result.AddRange(value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            return true;
        }
        else
        {
            result = null;
            return false;
        }
    }
}