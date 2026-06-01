using TheTinyApplicationLayer.Application.Domain;
using TheTinyApplicationLayer.Application.Persistence;
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
        var user = User.Create(
            command.UserId,
            command.Email,
            command.DisplayName,
            command.RegisteredAtUtc);

        dbContext.Users.Add(user);

        await events.PublishAsync(
            new UserRegistered(
                user.Id,
                user.Email,
                user.DisplayName,
                user.RegisteredAtUtc),
            ct);

        await dbContext.SaveChangesAsync(ct);
    }
}
