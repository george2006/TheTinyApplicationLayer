using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TheTinyApplicationLayer.Infrastructure.Persistence;
using TheTinyApplicationLayer.Infrastructure.Users;
using TinyEvents.SqlServer.EntityFrameworkCore;

namespace TheTinyApplicationLayer.Infrastructure.DependencyInjection;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ApplicationDb")
            ?? "Server=localhost,14333;Database=TheTinyApplicationLayer;User Id=sa;Password=TinyApplication_2026!;Encrypt=False;TrustServerCertificate=True;";

        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });

        services.UseSqlServerEntityFrameworkCoreOutbox<ApplicationDbContext>();
        services.AddScoped<EfCoreUserEmailLookup>();
        services.AddScoped<EfCoreWelcomeEmailLogWriter>();

        return services;
    }
}
