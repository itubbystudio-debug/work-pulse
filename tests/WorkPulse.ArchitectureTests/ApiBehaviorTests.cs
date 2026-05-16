using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class ApiBehaviorTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private static void AuthenticateAsAdmin(HttpClient client)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "test-admin-token");
    }

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
    public async Task SystemAdministration_Without_Admin_Token_Should_Return_Unauthorized()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/v1/systemadministration/records");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task SystemAdministration_Should_Create_And_List_Master_Data_Record()
    {
        using var client = factory.CreateClient();
        AuthenticateAsAdmin(client);
        var name = $"People Operations {Guid.NewGuid():N}";

        var createResponse = await client.PostAsJsonAsync("/api/v1/systemadministration/records", new
        {
            type = 9,
            name,
            code = $"DEP-{Guid.NewGuid():N}"[..12],
            description = "Owns workforce administration",
            parentId = (Guid?)null,
            isActive = true,
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createBody = await createResponse.Content.ReadAsStringAsync();
        createBody.Should().Contain(name);

        var listResponse = await client.GetAsync("/api/v1/systemadministration/records?type=9&isActive=true");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var listBody = await listResponse.Content.ReadAsStringAsync();
        listBody.Should().Contain(name, $"create response was {createBody}");
    }

    [Fact]
    public async Task SystemAdministration_With_Incomplete_Master_Data_Should_Return_ProblemDetails()
    {
        using var client = factory.CreateClient();
        AuthenticateAsAdmin(client);

        var response = await client.PostAsJsonAsync("/api/v1/systemadministration/records", new
        {
            type = 9,
            name = string.Empty,
            isActive = true,
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Name");
    }

    [Fact]
    public async Task SystemAdministration_Should_Grant_Action_Permission_For_Role_And_Menu()
    {
        using var client = factory.CreateClient();
        AuthenticateAsAdmin(client);

        var roleId = await CreateSystemAdministrationRecordAsync(client, 2, $"Admin Role {Guid.NewGuid():N}");
        var menuId = await CreateSystemAdministrationRecordAsync(client, 4, $"Leave Approval {Guid.NewGuid():N}");

        var grantResponse = await client.PostAsJsonAsync("/api/v1/systemadministration/access-control", new
        {
            subjectRecordId = roleId,
            screenRecordId = menuId,
            action = "Approve",
            isAllowed = true,
        });

        var grantBody = await grantResponse.Content.ReadAsStringAsync();
        grantResponse.StatusCode.Should().Be(HttpStatusCode.OK, grantBody);

        var matrixResponse = await client.GetAsync($"/api/v1/systemadministration/access-control?subjectRecordId={roleId}");
        matrixResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await matrixResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Approve");
        body.Should().Contain("Leave Approval");
    }

    [Fact]
    public async Task SystemAdministration_Should_Reject_Permission_For_Invalid_Subject()
    {
        using var client = factory.CreateClient();
        AuthenticateAsAdmin(client);

        var departmentId = await CreateSystemAdministrationRecordAsync(client, 9, $"Department {Guid.NewGuid():N}");
        var menuId = await CreateSystemAdministrationRecordAsync(client, 4, $"Attendance {Guid.NewGuid():N}");

        var response = await client.PostAsJsonAsync("/api/v1/systemadministration/access-control", new
        {
            subjectRecordId = departmentId,
            screenRecordId = menuId,
            action = "View",
            isAllowed = true,
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("InvalidPermissionSubject");
    }

    private static async Task<Guid> CreateSystemAdministrationRecordAsync(
        HttpClient client,
        int type,
        string name)
    {
        var response = await client.PostAsJsonAsync("/api/v1/systemadministration/records", new
        {
            type,
            name,
            code = $"R{Guid.NewGuid():N}"[..12],
            isActive = true,
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var json = await response.Content.ReadFromJsonAsync<JsonElement>();
        return json.GetProperty("data").GetProperty("id").GetGuid();
    }
}
