using System.Net;
using System.Net.Http.Json;
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
    public async Task CreateWorkType_With_Valid_Input_Should_Appear_In_List()
    {
        using var client = factory.CreateClient();
        var code = $"office-{Guid.NewGuid():N}";

        var createResponse = await client.PostAsJsonAsync("/api/v1/admin/work-types", new
        {
            code,
            name = "Office",
            description = "Office attendance",
            isActive = true,
            policySettingsJson = "{\"requiresLocation\":true}",
        });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var listResponse = await client.GetAsync("/api/v1/admin/work-types");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await listResponse.Content.ReadAsStringAsync();
        body.Should().Contain(code.ToUpperInvariant());
        body.Should().Contain("Office");
        body.Should().Contain("requiresLocation");
    }

    [Fact]
    public async Task UpdateWorkType_With_Valid_Input_Should_Save_Changes()
    {
        using var client = factory.CreateClient();
        var code = $"remote-{Guid.NewGuid():N}";

        var createResponse = await client.PostAsJsonAsync("/api/v1/admin/work-types", new
        {
            code,
            name = "Remote",
            isActive = true,
        });
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var createBody = await createResponse.Content.ReadAsStringAsync();
        using var document = System.Text.Json.JsonDocument.Parse(createBody);
        var id = document.RootElement.GetProperty("data").GetProperty("id").GetGuid();

        var updateResponse = await client.PutAsJsonAsync($"/api/v1/admin/work-types/{id}", new
        {
            code,
            name = "Remote Work",
            description = "Work outside office",
            isActive = false,
        });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var updateBody = await updateResponse.Content.ReadAsStringAsync();
        updateBody.Should().Contain("Remote Work");
        updateBody.Should().Contain("Work outside office");
        updateBody.Should().Contain("false");
    }

    [Fact]
    public async Task CreateWorkType_With_Invalid_Input_Should_Return_Validation_Feedback()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/admin/work-types", new
        {
            code = "bad code",
            name = string.Empty,
            isActive = true,
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Validation failed");
        body.Should().Contain("Code");
        body.Should().Contain("Name");
    }
}
