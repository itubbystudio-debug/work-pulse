using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Infrastructure.Identity;
using WorkPulse.Infrastructure.Persistence;
using WorkPulse.Infrastructure.Persistence.Interceptors;
using WorkPulse.Infrastructure.Services;

namespace WorkPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<AuditableEntityInterceptor>();
        services.AddScoped<DispatchDomainEventsInterceptor>();
        services.AddScoped<SoftDeleteInterceptor>();

        services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? "Server=(localdb)\\mssqllocaldb;Database=WorkPulse;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

            options.UseSqlServer(connectionString);
            options.AddInterceptors(
                serviceProvider.GetRequiredService<AuditableEntityInterceptor>(),
                serviceProvider.GetRequiredService<SoftDeleteInterceptor>(),
                serviceProvider.GetRequiredService<DispatchDomainEventsInterceptor>());
        });

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        services.AddScoped<ApplicationDbContextInitializer>();
        services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddScoped<IExcelReportService, ExcelReportService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<ICacheService, CacheService>();
        services.AddScoped<IdentityService>();

        return services;
    }
}
