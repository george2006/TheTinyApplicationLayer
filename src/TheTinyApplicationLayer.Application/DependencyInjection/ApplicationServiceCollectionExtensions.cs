using Microsoft.Extensions.DependencyInjection;
using TinyDispatcher;
using TinyDispatcher.Pipeline;
using TinyValidations;

namespace TheTinyApplicationLayer.Application.DependencyInjection;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.UseTinyValidations();
        services.AddTransient(typeof(TinyValidationMiddleware<>));

        services.UseTinyDispatcher<TinyDispatcher.AppContext>(tiny =>
        {
            tiny.UseGlobalMiddleware(typeof(TinyValidationMiddleware<>));
        });

        return services;
    }
}

public sealed class TinyValidationMiddleware<TCommand> : ICommandMiddleware<TCommand, TinyDispatcher.AppContext>
    where TCommand : ICommand
{
    private readonly ITinyValidator validator;

    public TinyValidationMiddleware(ITinyValidator validator)
    {
        this.validator = validator;
    }

    public async ValueTask InvokeAsync(
        TCommand command,
        TinyDispatcher.AppContext context,
        ICommandPipelineRuntime<TCommand, TinyDispatcher.AppContext> runtime,
        CancellationToken cancellationToken = default)
    {
        var result = await validator.ValidateAsync(command, cancellationToken);

        if (!result.IsValid)
        {
            throw new TinyValidationException(result.Errors);
        }

        await runtime.NextAsync(command, context, cancellationToken);
    }
}

public sealed class TinyValidationException : Exception
{
    public TinyValidationException(IReadOnlyCollection<ValidationError> errors)
        : base("The command did not pass validation.")
    {
        Errors = errors;
    }

    public IReadOnlyCollection<ValidationError> Errors { get; }
}
