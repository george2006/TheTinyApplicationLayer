namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed record UserRegistered(
    Guid UserId,
    string Email,
    string DisplayName,
    DateTimeOffset RegisteredAtUtc);
