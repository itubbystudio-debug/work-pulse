namespace WorkPulse.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddApiOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi();
        return services;
    }
}
