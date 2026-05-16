using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Infrastructure.Persistence;

namespace WorkPulse.ArchitectureTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"WorkPulseTests-{Guid.NewGuid()}";
    private readonly ServiceProvider _efServiceProvider = new ServiceCollection()
        .AddEntityFrameworkInMemoryDatabase()
        .BuildServiceProvider();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
            services.RemoveAll<ApplicationDbContext>();
            services.RemoveAll<IApplicationDbContext>();

            var sqlServerDescriptors = services
                .Where(descriptor =>
                    descriptor.ServiceType.Assembly.GetName().Name == "Microsoft.EntityFrameworkCore.SqlServer"
                    || descriptor.ImplementationType?.Assembly.GetName().Name == "Microsoft.EntityFrameworkCore.SqlServer")
                .ToArray();

            foreach (var descriptor in sqlServerDescriptors)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                options
                    .UseInMemoryDatabase(_databaseName)
                    .UseInternalServiceProvider(_efServiceProvider);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        });
    }
}
