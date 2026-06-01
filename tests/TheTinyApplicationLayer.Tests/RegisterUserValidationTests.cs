using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TheTinyApplicationLayer.Application.Users.RegisterUser;
using TheTinyApplicationLayer.Domain;
using TheTinyApplicationLayer.Infrastructure.Persistence;
using TheTinyApplicationLayer.Infrastructure.Users;
using TinyValidations;

namespace TheTinyApplicationLayer.Tests;

public sealed class RegisterUserValidationTests
{
    [Fact]
    public async Task Rejects_invalid_email()
    {
        using var provider = BuildProvider();

        var validator = provider.GetRequiredService<ITinyValidator>();
        var result = await validator.ValidateAsync(new RegisterUser(
            Guid.NewGuid(),
            "not-an-email",
            "Ada",
            DateTimeOffset.UtcNow));

        Assert.False(result.IsValid);
    }

    [Fact]
    public async Task Rejects_duplicate_email()
    {
        using var provider = BuildProvider();
        await SeedUserAsync(provider, "ada@example.com");

        var validator = provider.GetRequiredService<ITinyValidator>();
        var result = await validator.ValidateAsync(new RegisterUser(
            Guid.NewGuid(),
            "ada@example.com",
            "Ada",
            DateTimeOffset.UtcNow));

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, error => error.Member == nameof(RegisterUser.Email)
            && error.Message == "A user with this email already exists.");
    }

    private static ServiceProvider BuildProvider()
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseInMemoryDatabase(Guid.NewGuid().ToString());
        });
        services.AddScoped<EfCoreUserEmailLookup>();
        services.UseTinyValidations();

        return services.BuildServiceProvider();
    }

    private static async Task SeedUserAsync(
        ServiceProvider provider,
        string email)
    {
        await using var scope = provider.CreateAsyncScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Users.Add(User.Create(
            Guid.NewGuid(),
            email,
            "Ada",
            DateTimeOffset.UtcNow));

        await dbContext.SaveChangesAsync();
    }
}
