using Microsoft.EntityFrameworkCore;
using TheTinyApplicationLayer.Application.Users.RegisterUser;
using TheTinyApplicationLayer.Infrastructure.Persistence;
using TheTinyApplicationLayer.Infrastructure.Users;

namespace TheTinyApplicationLayer.Tests;

public sealed class CreateWelcomeEmailLogTests
{
    [Fact]
    public async Task Consumer_writes_welcome_log()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        await using var dbContext = new ApplicationDbContext(options);
        var consumer = new CreateWelcomeEmailLog(new EfCoreWelcomeEmailLogWriter(dbContext));

        await consumer.ConsumeAsync(
            new UserRegistered(Guid.NewGuid(), "ada@example.com", "Ada", DateTimeOffset.UtcNow),
            CancellationToken.None);

        var log = await dbContext.WelcomeEmailLogs.SingleAsync();

        Assert.Equal("ada@example.com", log.Email);
        Assert.Contains("Ada", log.Message);
    }
}
