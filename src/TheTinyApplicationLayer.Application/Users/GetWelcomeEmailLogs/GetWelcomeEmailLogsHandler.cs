using Microsoft.EntityFrameworkCore;
using TheTinyApplicationLayer.Application.Persistence;
using TinyDispatcher;

namespace TheTinyApplicationLayer.Application.Users.GetWelcomeEmailLogs;

public sealed class GetWelcomeEmailLogsHandler
    : IQueryHandler<GetWelcomeEmailLogs, IReadOnlyList<WelcomeEmailLog>>
{
    private const int MaximumCount = 50;

    private readonly ApplicationDbContext dbContext;

    public GetWelcomeEmailLogsHandler(ApplicationDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<WelcomeEmailLog>> HandleAsync(
        GetWelcomeEmailLogs query,
        CancellationToken ct = default)
    {
        var count = Math.Clamp(query.Count, 1, MaximumCount);

        return await dbContext.WelcomeEmailLogs
            .AsNoTracking()
            .OrderByDescending(log => log.CreatedAtUtc)
            .Take(count)
            .Select(log => new WelcomeEmailLog(
                log.UserId,
                log.Email,
                log.Message,
                log.CreatedAtUtc))
            .ToListAsync(ct);
    }
}

