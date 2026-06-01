# TinySuite sample notes

TheTinyApplicationLayer is the shared TinySuite sample.

It exists to show the three TinySuite NuGet packages working together in one ordinary application:

- TinyValidations validates the incoming command.
- TinyDispatcher dispatches the command to the use-case handler.
- TinyEvents stores and processes the resulting application event through an outbox.

The sample is intentionally small. It is not a framework, a template, or a recommended production architecture. It is a readable application-layer example that shows where each package belongs.

## Flow

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

The important part is the boundary between the use case and the durable event side effect. The command is validated before the handler runs. The handler publishes a `UserRegistered` event. TinyEvents records that event durably, and the worker processes it later.

## Why this sample uses NuGet packages

The sample should exercise TinySuite the way an application would consume it: through package references.

That keeps the integration honest. If the packages do not compose cleanly from NuGet, the sample should reveal that before a user hits it in their own application.
