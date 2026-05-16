using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace WorkPulse.Api.Authorization;

public sealed class PermissionHeaderAuthenticationHandler(
    IOptionsMonitor<PermissionHeaderAuthenticationOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<PermissionHeaderAuthenticationOptions>(options, logger, encoder)
{
    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.TryGetValue(PermissionHeaderAuthenticationDefaults.UserIdHeaderName, out var userIds)
            || string.IsNullOrWhiteSpace(userIds.FirstOrDefault()))
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userIds.First()!),
            new(ClaimTypes.Name, userIds.First()!),
        };

        if (Request.Headers.TryGetValue(PermissionHeaderAuthenticationDefaults.PermissionsHeaderName, out var permissions))
        {
            foreach (var permission in permissions
                         .SelectMany(value => value?.Split(
                             [',', ';', ' '],
                             StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries) ?? []))
            {
                claims.Add(new Claim(PermissionClaimTypes.Permission, permission));
            }
        }

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
