# Finance API

Repository for a finance API to store track and store stocks with lots and other personal finances.

## Development

Requires .NET 5.0 or newer SDK.

Application can be build with `dotnet build`.
Tests can be executed with `dotnet test`.

### Configuration

The `appsettings.development.json` should at all time contain all required fields necessary to run the application right away locally.

## TODO

- [x] Endpoint to receive latest stock quotes
- [ ] Ability to authenticate a user. (Thinking to use Github for authentication)
- [ ] Ability to mark a stock symbol as being tracked
- [ ] Ability to add a stock lot to indicate how many shares a user has of a given share and what it costs
