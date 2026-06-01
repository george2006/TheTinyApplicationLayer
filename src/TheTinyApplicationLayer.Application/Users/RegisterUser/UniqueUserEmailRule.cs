using TinyValidations;
using TheTinyApplicationLayer.Infrastructure.Users;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed class UniqueUserEmailRule : IAsyncValidationRule<RegisterUser>
{
    private readonly EfCoreUserEmailLookup users;

    public UniqueUserEmailRule(EfCoreUserEmailLookup users)
    {
        this.users = users;
    }

    public async ValueTask ValidateAsync(
        RegisterUser instance,
        ValidationErrorCollection errors,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(instance.Email))
        {
            return;
        }

        var email = instance.Email.Trim();

        if (await users.ExistsAsync(email, cancellationToken))
        {
            errors.Add(nameof(RegisterUser.Email), "A user with this email already exists.");
        }
    }
}
