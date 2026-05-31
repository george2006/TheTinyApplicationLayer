# TheTinyApplicationLayer

A small ASP.NET Core + Blazor sample showing the Tiny suite working together:

```text
Blazor Form
-> API Endpoint
-> TinyValidations
-> TinyDispatcher
-> Use Case
-> TinyEvents Outbox
-> Worker
-> Event Consumer
-> WelcomeEmailLog
```

This is not a framework or a production template. It is a readable sample of an explicit application layer.

## Why Docker Compose is required

This sample intentionally uses SQL Server through Docker Compose. TinyEvents is an outbox-first library, so the interesting behavior only appears when events are stored durably and later processed by a worker. Running against a real database allows the sample to demonstrate transaction boundaries, outbox storage, worker claiming, and event processing.

`dotnet run` is not enough. Start SQL Server first:

```bash
docker compose up -d
```

## Run the app

The local connection string is in `src/TheTinyApplicationLayer.Web/appsettings.json` and points at SQL Server on port `14333`.

```bash
dotnet restore
dotnet run --project src/TheTinyApplicationLayer.Web
```

In development, the sample uses `EnsureCreatedAsync` to create the schema. Production applications should use migrations. The EF Core model includes `Users`, `WelcomeEmailLogs`, and the TinyEvents outbox table through `modelBuilder.UseTinyEventsOutbox()`.

Open the app, submit the Register User form, then inspect the database.

## What to inspect in the database

After form submit:

- `Users` contains the registered user.
- `TinyOutbox` contains the serialized `UserRegistered` event.

After the worker runs:

- `WelcomeEmailLogs` contains the consumer side effect.
- The TinyEvents outbox row is marked processed according to TinyEvents behavior.

For this sample, the TinyEvents worker runs in the same ASP.NET host. In production, it could run in a separate worker process using the same database.

## Where the Tiny packages appear

- TinyValidations validates `RegisterUser`.
- TinyDispatcher dispatches `RegisterUser` to `RegisterUserHandler`.
- TinyEvents publishes `UserRegistered` into the outbox.
- TinyEvents.SqlServer.EntityFrameworkCore stores the outbox row with EF Core.
- TinyEvents.Worker claims and processes pending outbox rows.

Status: this sample uses TinyEvents alpha packages. APIs may change before TinyEvents 1.0.

## Package versions

Verified against nuget.org on May 31, 2026:

- `TinyValidations` `0.1.0-beta.1`
- `TinyDispatcher` `1.2.0-alpha.1`
- `TinyEvents` `0.1.0-alpha.1`
- `TinyEvents.SqlServer.EntityFrameworkCore` `0.1.0-alpha.1`
- `TinyEvents.Worker` `0.1.0-alpha.1`
