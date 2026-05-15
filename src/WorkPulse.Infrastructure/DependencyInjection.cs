using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Infrastructure.DataAccess;
using WorkPulse.Infrastructure.Persistence;
using WorkPulse.Infrastructure.Persistence.Repositories;
using WorkPulse.Infrastructure.Reporting;

namespace WorkPulse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<SqlServerOptions>(options =>
        {
            options.DefaultConnection = configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
        });

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped(provider =>
            new SqlConnectionFactory(provider.GetRequiredService<IOptions<SqlServerOptions>>().Value));

        services.AddScoped<ITechnologyCapabilityRepository, TechnologyCapabilityRepository>();
        services.AddScoped<IExcelReportService, ClosedXmlExcelReportService>();
        services.AddScoped<IRawSqlQueryCapability, DapperRawSqlQueryCapability>();

        return services;
    }
}
