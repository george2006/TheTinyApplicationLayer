namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public interface IUserEmailLookup
{
    ValueTask<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
}
