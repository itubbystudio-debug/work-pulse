using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Infrastructure.Persistence;

namespace WorkPulse.ArchitectureTests;

public sealed class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration(configuration =>
        {
            configuration.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["SystemAdministration:AdminBearerToken"] = "test-admin-token",
            });
        });

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

            var databaseName = $"WorkPulseTests-{Guid.NewGuid()}";
            var databaseRoot = new InMemoryDatabaseRoot();

            services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var efServiceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

                options
                    .UseInMemoryDatabase(databaseName, databaseRoot)
                    .UseInternalServiceProvider(efServiceProvider);
            });

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        });
    }
}
