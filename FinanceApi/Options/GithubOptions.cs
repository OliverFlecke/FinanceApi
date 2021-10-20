namespace FinanceApi.Options;

public class GithubOptions
{
    public const string Github = "Github";

    #pragma warning disable CS8618 // Consider declaring the property as nullable.
    public string ClientId { get; set; }

    public string ClientSecret { get; set; }
    #pragma warning restore CS8618 // Consider declaring the property as nullable.
}