# The Tiny Application Layer Manifesto

Tiny is about application ownership: small contracts, honest boundaries, and code teams can understand.

The goal of Tiny is not to make applications clever. The goal is to make the important flow visible.

We prefer small application infrastructure that teams can understand and own. We prefer explicit flow over hidden framework behavior. We prefer boring runtime over magical runtime.

Source generation is useful when it removes mechanical registration code. It should not turn the runtime into a guessing game.

Transaction boundaries should be honest. In this sample, the user row and TinyEvents outbox row are written through the same EF Core `DbContext` and saved together.

Side effects that matter should be durable. The welcome-email action is not called synchronously from the use case. It is published as an application event, stored in the outbox, claimed by a worker, and processed later.

Provider details belong near infrastructure. The application layer works with small interfaces and event contracts; SQL Server and EF Core stay in the infrastructure project.

Tests should prove behavior, not private trivia.

These libraries are not intended to replace every mature enterprise tool. They are a small, explicit way to build application-layer flow when that is enough.
