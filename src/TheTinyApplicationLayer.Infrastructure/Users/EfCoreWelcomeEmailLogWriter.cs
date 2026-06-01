using TheTinyApplicationLayer.Application.Persistence;
using TheTinyApplicationLayer.Application.Users;
using TheTinyApplicationLayer.Application.Users.RegisterUser;

namespace TheTinyApplicationLayer.Infrastructure.Users;

public sealed class EfCoreWelcomeEmailLogWriter : IWelcomeEmailLogWriter
{
    private readonly ApplicationDbContext dbContext;

    public EfCoreWelcomeEmailLogWriter(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(Guid userId, string email, string message, DateTimeOffset createdAtUtc)
    {
        dbContext.WelcomeEmailLogs.Add(WelcomeEmailLog.Create(
            userId,
            email,
            message,
            createdAtUtc));
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
