using TinyDispatcher;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed record RegisterUser(
    Guid UserId,
    string Email,
    string DisplayName,
    DateTimeOffset RegisteredAtUtc) : ICommand;
