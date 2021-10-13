using Microsoft.Extensions.Logging;

namespace FinanceApi.Test.Logging;

sealed class XUnitLoggerProvider : ILoggerProvider
{
    readonly ITestOutputHelper _testOutputHelper;
    readonly LoggerExternalScopeProvider _scopeProvider = new();

    public XUnitLoggerProvider(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public ILogger CreateLogger(string categoryName) =>
        new XUnitLogger(_testOutputHelper, _scopeProvider, categoryName);

    public void Dispose() { }
}