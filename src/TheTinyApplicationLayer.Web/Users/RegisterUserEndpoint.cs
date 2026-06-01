using TheTinyApplicationLayer.Application.DependencyInjection;
using TheTinyApplicationLayer.Application.Users.GetWelcomeEmailLogs;
using TheTinyApplicationLayer.Application.Users.RegisterUser;
using TinyDispatcher.Dispatching;

namespace TheTinyApplicationLayer.Web.Users;

public static class RegisterUserEndpoint
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/users/register", async (
            RegisterUserRequest request,
            IDispatcher<TinyDispatcher.AppContext> dispatcher,
            CancellationToken cancellationToken) =>
        {
            var userId = Guid.NewGuid();
            var registeredAtUtc = DateTimeOffset.UtcNow;

            await dispatcher.DispatchAsync(
                new RegisterUser(
                    userId,
                    request.Email,
                    request.DisplayName,
                    registeredAtUtc),
                cancellationToken);

            return Results.Created($"/api/users/{userId}", new RegisterUserResponse(userId));
        });

        app.MapGet("/api/welcome-email-logs", async (
            IDispatcher<TinyDispatcher.AppContext> dispatcher,
            CancellationToken cancellationToken) =>
        {
            var logs = await dispatcher.DispatchAsync<GetWelcomeEmailLogs, IReadOnlyList<WelcomeEmailLog>>(
                new GetWelcomeEmailLogs(10),
                cancellationToken);

            return Results.Ok(logs);
        });

        return app;
    }

}

public sealed record RegisterUserRequest(string Email, string DisplayName);

public sealed record RegisterUserResponse(Guid UserId);

public static class TinyValidationProblemDetailsMiddleware
{
    public static IApplicationBuilder UseTinyValidationProblemDetails(this IApplicationBuilder app)
    {
        return app.Use(async (context, next) =>
        {
            try
            {
                await next(context);
            }
            catch (TinyValidationException exception)
            {
                var errors = exception.Errors
                    .GroupBy(error => error.Member)
                    .ToDictionary(
                        group => group.Key,
                        group => group.Select(error => error.Message).ToArray());

                await Results.ValidationProblem(errors).ExecuteAsync(context);
            }
        });
    }
}
