namespace TheTinyApplicationLayer.Domain;

public sealed class WelcomeEmailLog
{
    private WelcomeEmailLog()
    {
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public DateTimeOffset CreatedAtUtc { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public static WelcomeEmailLog Create(
        Guid userId,
        string email,
        string message,
        DateTimeOffset createdAtUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("User id is required.", nameof(userId));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(message))
        {
            throw new ArgumentException("Message is required.", nameof(message));
        }

        if (createdAtUtc == default)
        {
            throw new ArgumentException("Creation time is required.", nameof(createdAtUtc));
        }

        return new WelcomeEmailLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Email = email.Trim(),
            Message = message.Trim(),
            CreatedAtUtc = createdAtUtc
        };
    }
}
