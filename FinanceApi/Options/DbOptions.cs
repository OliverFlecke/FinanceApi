namespace FinanceApi.Options;

public class DbOptions
{
    public const string SectionName = "DB";

    public required string Host { get; init; }
    public required string User { get; init; }
    public required string Password { get; init; }
    public required string Database { get; init; }
    public required string Port { get; init; }

    public string ConnectionString => $"host={Host};database={Database};user id={User};password={Password};port={Port}";
}