using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkPulse.Application.Abstractions.Data;
using WorkPulse.Application.Abstractions.Persistence;
using WorkPulse.Infrastructure.Data;
using WorkPulse.Infrastructure.Persistence;

namespace WorkPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<DbContextOptionsBuilder>? configureDbContext = null)
    {
        services.AddDbContext<WorkPulseDbContext>(options =>
        {
            if (configureDbContext is not null)
            {
                configureDbContext(options);
                return;
            }

            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
            }

            options.UseSqlServer(connectionString);
        });

        services.AddScoped<IWorkspaceProfileRepository, WorkspaceProfileRepository>();
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();

        return services;
    }
}
