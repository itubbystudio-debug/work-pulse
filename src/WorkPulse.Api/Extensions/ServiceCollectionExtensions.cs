using Microsoft.AspNetCore.Authorization;
using WorkPulse.Api.Authorization;

namespace WorkPulse.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddProblemDetails();
        services.AddHttpContextAccessor();

        services
            .AddAuthentication(PermissionHeaderAuthenticationDefaults.AuthenticationScheme)
            .AddScheme<PermissionHeaderAuthenticationOptions, PermissionHeaderAuthenticationHandler>(
                PermissionHeaderAuthenticationDefaults.AuthenticationScheme,
                options => { });

        services.AddAuthorization();
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        return services;
    }
}
