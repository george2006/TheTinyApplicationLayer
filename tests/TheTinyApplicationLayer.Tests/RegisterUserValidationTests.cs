using Microsoft.Extensions.DependencyInjection;
using TheTinyApplicationLayer.Application.Users.RegisterUser;
using TinyValidations;

namespace TheTinyApplicationLayer.Tests;

public sealed class RegisterUserValidationTests
{
    [Fact]
    public async Task Rejects_invalid_email()
    {
        var services = new ServiceCollection();
        services.AddSingleton<IUserEmailLookup>(new StubUserEmailLookup(false));
        services.UseTinyValidations();
        using var provider = services.BuildServiceProvider();

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
        var services = new ServiceCollection();
        services.AddSingleton<IUserEmailLookup>(new StubUserEmailLookup(true));
        services.UseTinyValidations();
        using var provider = services.BuildServiceProvider();

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

    private sealed class StubUserEmailLookup : IUserEmailLookup
    {
        private readonly bool exists;

        public StubUserEmailLookup(bool exists)
        {
            this.exists = exists;
        }

        public ValueTask<bool> ExistsAsync(
            string email,
            CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(exists);
        }
    }
}
