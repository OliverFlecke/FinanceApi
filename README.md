# Finance API

Repository for a finance API to store track and store stocks with lots and other personal finances.

## Development

Requires .NET 6.0 or newer SDK.

Application can now be run in hot reload mode with `dotnet watch run`.
This is the recommended way to run the application during development.

Application can be build with `dotnet build`.
Tests can be executed with `dotnet test`.

### Building Docker image

A Dockerfile is provided in the `FinanceApi` directory, which can be used to build the application.

Note that if building on a ARM processor (e.g. Apple Silicon), you will need to use the `buildx` option to target `amd`.
This command will build and push the image:

```sh
docker buildx build  --platform linux/amd64 -t oliverflecke/financeapi:latest . --file FinanceApi/Dockerfile --push
```

### Configuration

The `appsettings.development.json` should at all time contain all required fields necessary to run the application right away locally.

## TODO

- [x] Endpoint to receive latest stock quotes
- [x] Ability to authenticate a user. (Thinking to use Github for authentication)
- [x] Ability to mark a stock symbol as being tracked
- [x] Ability to add a stock lot to indicate how many shares a user has of a given share and what it costs
