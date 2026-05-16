using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using WorkPulse.Domain.Entities;
using WorkPulse.Infrastructure.Persistence;

namespace WorkPulse.ArchitectureTests;

public sealed class AdminRolePermissionsApiTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task AssignRolePermissions_With_Valid_Permissions_Should_Persist_And_Be_Retrievable()
    {
        using var client = factory.CreateClient();
        var (roleId, permissionIds) = await SeedRoleAndPermissionsAsync(
            new Permission("workspaces.read", "Read workspaces"),
            new Permission("workspaces.update", "Update workspaces"));

        var assignResponse = await client.PutAsJsonAsync(
            $"/api/v1/admin/roles/{roleId}/permissions",
            new { permissionIds });

        assignResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await client.GetAsync($"/api/v1/admin/roles/{roleId}/permissions");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadAsStringAsync();
        body.Should().Contain(roleId.ToString());
        body.Should().Contain("workspaces.read");
        body.Should().Contain("workspaces.update");
    }

    [Fact]
    public async Task AssignRolePermissions_With_Invalid_Permission_Should_Return_BadRequest()
    {
        using var client = factory.CreateClient();
        var (roleId, permissionIds) = await SeedRoleAndPermissionsAsync(
            new Permission("workspaces.read", "Read workspaces"));
        var invalidPermissionId = Guid.NewGuid();

        var assignResponse = await client.PutAsJsonAsync(
            $"/api/v1/admin/roles/{roleId}/permissions",
            new { permissionIds = permissionIds.Append(invalidPermissionId).ToArray() });

        assignResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        assignResponse.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await assignResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Permission.InvalidReference");
        body.Should().Contain(invalidPermissionId.ToString());
    }

    private async Task<(Guid RoleId, Guid[] PermissionIds)> SeedRoleAndPermissionsAsync(
        params Permission[] permissions)
    {
        using var scope = factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var role = new Role($"Admin-{Guid.NewGuid():N}");
        dbContext.Roles.Add(role);
        dbContext.Permissions.AddRange(permissions);

        await dbContext.SaveChangesAsync();

        return (role.Id, permissions.Select(permission => permission.Id).ToArray());
    }
}
