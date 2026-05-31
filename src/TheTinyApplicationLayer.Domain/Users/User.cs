namespace TheTinyApplicationLayer.Domain.Users;

public sealed class User
{
    public Guid Id { get; init; }

    public string Email { get; init; } = string.Empty;

    public string DisplayName { get; init; } = string.Empty;

    public DateTimeOffset RegisteredAtUtc { get; init; }
}
