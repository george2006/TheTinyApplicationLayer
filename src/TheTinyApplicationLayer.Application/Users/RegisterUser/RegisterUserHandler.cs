using TheTinyApplicationLayer.Application.Persistence;
using TheTinyApplicationLayer.Application.Users;
using TinyDispatcher;
using TinyEvents;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed class RegisterUserHandler
    : ICommandHandler<RegisterUser, TinyDispatcher.AppContext>
{
    private readonly ApplicationDbContext dbContext;
    private readonly ITinyEventPublisher events;

    public RegisterUserHandler(
        ApplicationDbContext dbContext,
        ITinyEventPublisher events)
    {
        this.dbContext = dbContext;
        this.events = events;
    }

    public async Task HandleAsync(
        RegisterUser command,
        TinyDispatcher.AppContext ctx,
        CancellationToken ct = default)
    {
        var email = command.Email.Trim();
        var displayName = command.DisplayName.Trim();

        dbContext.Users.Add(new UserRow
        {
            Id = command.UserId,
            Email = email,
            DisplayName = displayName,
            RegisteredAtUtc = command.RegisteredAtUtc
        });

        await events.PublishAsync(
            new UserRegistered(
                command.UserId,
                email,
                displayName,
                command.RegisteredAtUtc),
            ct);

        await dbContext.SaveChangesAsync(ct);
    }
}
