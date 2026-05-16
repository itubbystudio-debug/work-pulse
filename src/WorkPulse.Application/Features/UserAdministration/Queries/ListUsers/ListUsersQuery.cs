using WorkPulse.Application.Common.Messaging;

namespace WorkPulse.Application.Features.UserAdministration.Queries.ListUsers;

public sealed record ListUsersQuery(string? Search = null, bool? IsActive = null)
    : IQuery<IReadOnlyList<UserAdministrationUserDto>>;

public sealed record UserAdministrationUserDto(
    Guid Id,
    string IdentityUserId,
    string UserName,
    string Email,
    string DisplayName,
    string Role,
    bool IsActive);
