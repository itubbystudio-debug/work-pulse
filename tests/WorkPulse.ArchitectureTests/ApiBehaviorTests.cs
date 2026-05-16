using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class ApiBehaviorTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task GetArchitectureSummary_Should_Return_Configured_Backend_Stack()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/diagnostics/architecture");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain(".NET 10 Web API");
        body.Should().Contain("MediatR");
        body.Should().Contain("SQL Server");
        body.Should().Contain("EF Core");
        body.Should().Contain("Dapper");
        body.Should().Contain("ClosedXML");
    }

    [Fact]
    public async Task GetArchitectureSummary_Should_Return_Configured_Frontend_Stack()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/diagnostics/architecture");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Angular");
        body.Should().Contain("PrimeNG");
        body.Should().Contain("Sakai Template");
        body.Should().Contain("Form");
        body.Should().Contain("Table");
        body.Should().Contain("Button");
        body.Should().Contain("Card");
        body.Should().Contain("Modal");
    }

    [Fact]
    public async Task GetArchitectureSummary_Should_Flag_Non_Approved_Ui_Libraries_For_Review()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/diagnostics/architecture");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Non-approved UI libraries");
        body.Should().Contain("flagged for review");
        body.Should().Contain("explicit approval");
    }

    [Fact]
    public async Task GetArchitectureSummary_Should_Define_Legacy_Migration_Or_Exception_Policy()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/diagnostics/architecture");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Mixed legacy frontend modules");
        body.Should().Contain("migration");
        body.Should().Contain("exception");
    }

    [Fact]
    public async Task CreateWorkspace_With_Invalid_Input_Should_Return_ProblemDetails()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/diagnostics/workspaces", new { name = string.Empty });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Validation failed");
        body.Should().Contain("Name");
    }

    [Fact]
    public async Task ExportWorkspacesReport_Should_Return_Excel_File()
    {
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync("/api/v1/diagnostics/workspaces", new { name = "Architecture" });
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var reportResponse = await client.GetAsync("/api/v1/diagnostics/workspaces/report");

        reportResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        reportResponse.Content.Headers.ContentType?.MediaType
            .Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        var content = await reportResponse.Content.ReadAsByteArrayAsync();
        content.Should().NotBeEmpty();
        content.Take(2).Should().Equal(0x50, 0x4B);
    }

    [Fact]
    public async Task CreateUser_With_Admin_Should_Save_And_Appear_In_Admin_List()
    {
        using var client = factory.CreateClient();
        AddAdminHeaders(client);

        var createResponse = await client.PostAsJsonAsync("/api/v1/admin/users", new
        {
            identityUserId = "identity-001",
            userName = "admin.user",
            email = "admin.user@example.com",
            displayName = "Admin User",
            role = "Admin",
            isActive = true,
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createBody = await createResponse.Content.ReadAsStringAsync();
        createBody.Should().Contain("admin.user@example.com");

        var listResponse = await client.GetAsync("/api/v1/admin/users");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var listBody = await listResponse.Content.ReadAsStringAsync();
        listBody.Should().Contain("admin.user");
        listBody.Should().Contain("Admin User");
    }

    [Fact]
    public async Task CreateUser_With_Incomplete_Required_Data_Should_Return_ProblemDetails()
    {
        using var client = factory.CreateClient();
        AddAdminHeaders(client);

        var response = await client.PostAsJsonAsync("/api/v1/admin/users", new
        {
            identityUserId = string.Empty,
            userName = string.Empty,
            email = string.Empty,
            displayName = string.Empty,
            role = string.Empty,
            isActive = true,
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Validation failed");
        body.Should().Contain("IdentityUserId");
        body.Should().Contain("UserName");
        body.Should().Contain("Email");
        body.Should().Contain("DisplayName");
        body.Should().Contain("Role");
    }

    [Fact]
    public async Task AdminUsers_Without_Admin_Role_Should_Be_Denied()
    {
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-User-Id", "user-001");
        client.DefaultRequestHeaders.Add("X-User-Name", "standard.user");
        client.DefaultRequestHeaders.Add("X-User-Roles", "User");

        var response = await client.GetAsync("/api/v1/admin/users");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateUser_With_Admin_Should_Save_Changes_And_Activation()
    {
        using var client = factory.CreateClient();
        AddAdminHeaders(client);

        var createResponse = await client.PostAsJsonAsync("/api/v1/admin/users", new
        {
            identityUserId = "identity-002",
            userName = "ops.user",
            email = "ops.user@example.com",
            displayName = "Ops User",
            role = "Operator",
            isActive = true,
        });
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createBody = await createResponse.Content.ReadAsStringAsync();
        using var json = JsonDocument.Parse(createBody);
        var id = json.RootElement.GetProperty("data").GetProperty("id").GetGuid();

        var updateResponse = await client.PutAsJsonAsync($"/api/v1/admin/users/{id}", new
        {
            userName = "ops.user.updated",
            email = "ops.user.updated@example.com",
            displayName = "Ops User Updated",
            role = "Supervisor",
        });
        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var activationResponse = await client.PatchAsJsonAsync($"/api/v1/admin/users/{id}/activation", new
        {
            isActive = false,
        });
        activationResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var listResponse = await client.GetAsync("/api/v1/admin/users?isActive=false");
        var listBody = await listResponse.Content.ReadAsStringAsync();
        listBody.Should().Contain("ops.user.updated@example.com");
        listBody.Should().Contain("Supervisor");
    }

    private static void AddAdminHeaders(HttpClient client)
    {
        client.DefaultRequestHeaders.Add("X-User-Id", "admin-001");
        client.DefaultRequestHeaders.Add("X-User-Name", "admin");
        client.DefaultRequestHeaders.Add("X-User-Roles", "Admin");
    }
}
