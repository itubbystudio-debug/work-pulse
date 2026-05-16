using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class RoleAdministrationApiTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task Create_List_And_Update_Role_Should_Persist_Role_Definition_With_Stable_Identifier()
    {
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/v1/roles", new
        {
            code = "HR_ADMIN",
            name = "HR Admin",
            description = "Can administer HR data.",
            isActive = true,
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = JsonNode.Parse(await createResponse.Content.ReadAsStringAsync())!;
        var roleId = Guid.Parse(created["data"]!["id"]!.GetValue<string>());
        created["data"]!["code"]!.GetValue<string>().Should().Be("HR_ADMIN");

        var listResponse = await client.GetAsync("/api/v1/roles");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var list = await listResponse.Content.ReadAsStringAsync();
        list.Should().Contain("HR_ADMIN");
        list.Should().Contain("HR Admin");

        var updateResponse = await client.PutAsJsonAsync($"/api/v1/roles/{roleId}", new
        {
            name = "People Admin",
            description = "Can administer people data.",
            isActive = false,
        });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updated = JsonNode.Parse(await updateResponse.Content.ReadAsStringAsync())!;
        updated["data"]!["id"]!.GetValue<string>().Should().Be(roleId.ToString());
        updated["data"]!["code"]!.GetValue<string>().Should().Be("HR_ADMIN");
        updated["data"]!["name"]!.GetValue<string>().Should().Be("People Admin");
        updated["data"]!["isActive"]!.GetValue<bool>().Should().BeFalse();
    }

    [Fact]
    public async Task CreateRole_With_Missing_Required_Field_Should_Return_Validation_Feedback()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/roles", new
        {
            code = "FINANCE",
            name = string.Empty,
            description = "Missing name.",
            isActive = true,
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Validation failed");
        body.Should().Contain("Name");
    }

    [Fact]
    public async Task CreateRole_With_Duplicate_Name_Should_Return_Validation_Feedback()
    {
        using var client = factory.CreateClient();

        var firstResponse = await client.PostAsJsonAsync("/api/v1/roles", new
        {
            code = "OPS_ADMIN",
            name = "Operations Admin",
            description = "First role.",
            isActive = true,
        });
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var duplicateResponse = await client.PostAsJsonAsync("/api/v1/roles", new
        {
            code = "OPS_MANAGER",
            name = "Operations Admin",
            description = "Duplicate name.",
            isActive = true,
        });

        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await duplicateResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Role.NameDuplicate");
        body.Should().Contain("Role name already exists.");
    }
}
