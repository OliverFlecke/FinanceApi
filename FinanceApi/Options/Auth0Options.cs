namespace FinanceApi.Options;

public class Auth0Options
{
    public const string SectionName = "Auth0";

    public required string Domain { get; init; }
    public required string Audience { get; init; }
}