using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Infrastructure.Persistence;

namespace WorkPulse.ArchitectureTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly string _databaseName = $"WorkPulseTests-{Guid.NewGuid()}";
    private readonly InMemoryDatabaseRoot _databaseRoot = new();

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
                var efServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                options
                    .UseInMemoryDatabase(_databaseName, _databaseRoot)
                    .UseInternalServiceProvider(efServiceProvider);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        });
    }
}
