namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public interface IWelcomeEmailLogWriter
{
    void Add(Guid userId, string email, string message, DateTimeOffset createdAtUtc);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
