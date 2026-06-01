using TinyEvents;
using TheTinyApplicationLayer.Infrastructure.Users;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed class CreateWelcomeEmailLog : IEventConsumer<UserRegistered>
{
    private readonly EfCoreWelcomeEmailLogWriter logs;

    public CreateWelcomeEmailLog(EfCoreWelcomeEmailLogWriter logs)
    {
        this.logs = logs;
    }

    public async ValueTask ConsumeAsync(
        UserRegistered @event,
        CancellationToken cancellationToken)
    {
        logs.Add(
            @event.UserId,
            @event.Email,
            $"Welcome message queued for {@event.DisplayName}.",
            DateTimeOffset.UtcNow);

        await logs.SaveChangesAsync(cancellationToken);
    }
}
