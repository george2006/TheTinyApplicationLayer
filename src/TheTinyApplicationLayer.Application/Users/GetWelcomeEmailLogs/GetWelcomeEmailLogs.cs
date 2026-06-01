using TinyDispatcher;

namespace TheTinyApplicationLayer.Application.Users.GetWelcomeEmailLogs;

public sealed record GetWelcomeEmailLogs(int Count) : IQuery<IReadOnlyList<WelcomeEmailLog>>;

