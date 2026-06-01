namespace TheTinyApplicationLayer.Application.Domain;

public sealed class User
{
    private User()
    {
    }

    public Guid Id { get; private set; }

    public string Email { get; private set; } = string.Empty;

    public string DisplayName { get; private set; } = string.Empty;

    public DateTimeOffset RegisteredAtUtc { get; private set; }

    public static User Create(
        Guid id,
        string email,
        string displayName,
        DateTimeOffset registeredAtUtc)
    {
        if (id == Guid.Empty)
        {
            throw new ArgumentException("User id is required.", nameof(id));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email is required.", nameof(email));
        }

        if (string.IsNullOrWhiteSpace(displayName))
        {
            throw new ArgumentException("Display name is required.", nameof(displayName));
        }

        if (registeredAtUtc == default)
        {
            throw new ArgumentException("Registration time is required.", nameof(registeredAtUtc));
        }

        return new User
        {
            Id = id,
            Email = email.Trim(),
            DisplayName = displayName.Trim(),
            RegisteredAtUtc = registeredAtUtc
        };
    }
}
