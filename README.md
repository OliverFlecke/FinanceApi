# Finance API

Repository for a finance API to store track and store stocks with lots and other personal finances.

## Development

Requires .NET 7.0 or newer SDK.

Application can now be run in hot reload mode with `dotnet watch run`.
This is the recommended way to run the application during development.

Application can be build with `dotnet build`.
Tests can be executed with `dotnet test`.

The application is dependent on the postgres backend to do anything interesting.
To start a local db through [docker](https://docker.com) use:

```sh
docker run -d -p 5432:5432 -e POSTGRES_PASSWORD=password postgres:alpine
```

Afterwards you can use `dotnet ef database update` from the `FinanceApi` directory to apply all migrations.

### Loading data into the container

If you have a sql file with data to load into the database (for example retrieved with `pg_dump`), then you can use `docker cp <filename> <container name>:<filename>` to copy the file into the docker container.
Then use `docker exec -it <container name> sh` to enter the container.
Lastly, use `psql -f <filename>` to run the script.

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
