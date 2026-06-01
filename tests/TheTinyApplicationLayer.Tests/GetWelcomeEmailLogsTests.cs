using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheTinyApplicationLayer.Application.DependencyInjection;
using TheTinyApplicationLayer.Application.Persistence;
using TheTinyApplicationLayer.Application.Users;
using TheTinyApplicationLayer.Application.Users.GetWelcomeEmailLogs;
using TinyDispatcher.Dispatching;

namespace TheTinyApplicationLayer.Tests;

public sealed class GetWelcomeEmailLogsTests
{
    [Fact]
    public async Task Query_dispatches_through_TinyDispatcher()
    {
        var databaseName = Guid.NewGuid().ToString();
        var services = new ServiceCollection();
        services.AddApplication();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName);
        });

        using var provider = services.BuildServiceProvider();

        await SeedAsync(provider, "query@example.com", DateTimeOffset.UtcNow);

        var dispatcher = provider.GetRequiredService<IDispatcher<TinyDispatcher.AppContext>>();

        var logs = await dispatcher.DispatchAsync<GetWelcomeEmailLogs, IReadOnlyList<WelcomeEmailLog>>(
            new GetWelcomeEmailLogs(10),
            CancellationToken.None);

        var log = Assert.Single(logs);
        Assert.Equal("query@example.com", log.Email);
    }

    [Fact]
    public async Task Handler_returns_latest_welcome_logs()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ApplicationDbContext(options);

        dbContext.WelcomeEmailLogs.AddRange(
            CreateLog("old@example.com", DateTimeOffset.UtcNow.AddMinutes(-10)),
            CreateLog("new@example.com", DateTimeOffset.UtcNow));

        await dbContext.SaveChangesAsync();

        var handler = new GetWelcomeEmailLogsHandler(dbContext);

        var logs = await handler.HandleAsync(new GetWelcomeEmailLogs(1));

        var log = Assert.Single(logs);
        Assert.Equal("new@example.com", log.Email);
    }

    private static WelcomeEmailLogRow CreateLog(string email, DateTimeOffset createdAtUtc)
    {
        return new WelcomeEmailLogRow
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Email = email,
            Message = $"Welcome message queued for {email}.",
            CreatedAtUtc = createdAtUtc
        };
    }

    private static async Task SeedAsync(
        ServiceProvider provider,
        string email,
        DateTimeOffset createdAtUtc)
    {
        await using var scope = provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.WelcomeEmailLogs.Add(CreateLog(email, createdAtUtc));

        await dbContext.SaveChangesAsync();
    }
}
