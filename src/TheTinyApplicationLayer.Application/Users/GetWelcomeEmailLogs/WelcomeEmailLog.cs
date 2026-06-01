namespace TheTinyApplicationLayer.Application.Users.GetWelcomeEmailLogs;

public sealed record WelcomeEmailLog(
    Guid UserId,
    string Email,
    string Message,
    DateTimeOffset CreatedAtUtc);

