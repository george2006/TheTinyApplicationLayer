using TheTinyApplicationLayer.Application.Users.RegisterUser;
using TheTinyApplicationLayer.Domain.Users;
using TheTinyApplicationLayer.Infrastructure.Persistence;

namespace TheTinyApplicationLayer.Infrastructure.Users;

public sealed class EfCoreUserRegistrationStore : IUserRegistrationStore
{
    private readonly ApplicationDbContext dbContext;

    public EfCoreUserRegistrationStore(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public void Add(User user)
    {
        dbContext.Users.Add(new UserRow
        {
            Id = user.Id,
            Email = user.Email,
            DisplayName = user.DisplayName,
            RegisteredAtUtc = user.RegisteredAtUtc
        });
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
