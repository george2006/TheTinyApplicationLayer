using TinyValidations;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public sealed class RegisterUserValidation : IValidation<RegisterUser>
{
    public void Define(ValidationRules<RegisterUser> rules)
    {
        rules.Required(x => x.Email);
        rules.Email(x => x.Email);
        rules.HasText(x => x.DisplayName);
        rules.TextLengthAtLeast(x => x.DisplayName, 2);
        rules.Use<UniqueUserEmailRule>();
    }
}
