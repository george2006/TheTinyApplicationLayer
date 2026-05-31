using TheTinyApplicationLayer.Domain.Users;
using TinyDispatcher;
using TinyEvents;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed class RegisterUserHandler : ICommandHandler<RegisterUser, TinyDispatcher.AppContext>
{
    private readonly IUserRegistrationStore users;
    private readonly ITinyEventPublisher events;

    public RegisterUserHandler(
        IUserRegistrationStore users,
        ITinyEventPublisher events)
    {
        this.users = users;
        this.events = events;
    }

    public async Task HandleAsync(
        RegisterUser command,
        TinyDispatcher.AppContext ctx,
        CancellationToken ct = default)
    {
        users.Add(new User
        {
            Id = command.UserId,
            Email = command.Email.Trim(),
            DisplayName = command.DisplayName.Trim(),
            RegisteredAtUtc = command.RegisteredAtUtc
        });

        await events.PublishAsync(
            new UserRegistered(
                command.UserId,
                command.Email.Trim(),
                command.DisplayName.Trim(),
                command.RegisteredAtUtc),
            ct);

        await users.SaveChangesAsync(ct);
    }
}
