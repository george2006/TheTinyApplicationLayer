using TheTinyApplicationLayer.Domain.Users;

namespace TheTinyApplicationLayer.Application.Users.RegisterUser;

public interface IUserRegistrationStore
{
    void Add(User user);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
