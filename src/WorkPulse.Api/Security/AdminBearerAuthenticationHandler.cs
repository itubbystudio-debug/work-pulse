using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WorkPulse.Api.Security;

public sealed class AdminBearerAuthenticationHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "AdminBearer";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authorization)
            || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var token = authorization["Bearer ".Length..].Trim();
        var configuredToken = Context.RequestServices
            .GetRequiredService<IConfiguration>()
            .GetValue<string>("SystemAdministration:AdminBearerToken");

        if (string.IsNullOrWhiteSpace(configuredToken) || token != configuredToken)
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid administrator bearer token."));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "system-admin"),
            new Claim(ClaimTypes.Name, "System Administrator"),
            new Claim(ClaimTypes.Role, "Administrator"),
        };

        var identity = new ClaimsIdentity(claims, SchemeName);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, SchemeName);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
