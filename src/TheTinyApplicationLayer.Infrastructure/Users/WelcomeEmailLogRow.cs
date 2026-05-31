namespace TheTinyApplicationLayer.Infrastructure.Users;

public sealed class WelcomeEmailLogRow
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Email { get; set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; set; }

    public string Message { get; set; } = string.Empty;
}
