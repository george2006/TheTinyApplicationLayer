using Microsoft.EntityFrameworkCore;
using TheTinyApplicationLayer.Infrastructure.Persistence;

namespace TheTinyApplicationLayer.Infrastructure.Users;

public sealed class EfCoreUserEmailLookup
{
    private readonly ApplicationDbContext dbContext;

    public EfCoreUserEmailLookup(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async ValueTask<bool> ExistsAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var normalizedEmail = email.Trim();

        return await dbContext.Users.AnyAsync(
            user => user.Email == normalizedEmail,
            cancellationToken);
    }
}
