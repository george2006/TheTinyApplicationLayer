namespace TheTinyApplicationLayer.Application.Users;

public sealed class UserRow
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public DateTimeOffset RegisteredAtUtc { get; set; }
}

