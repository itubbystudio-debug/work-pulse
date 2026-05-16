using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Infrastructure.Identity;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public string? UserId => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
}
