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
    public async Task MasterData_CreateValidRecords_Should_Persist_Department_Position_And_EmployeeGroup()
    {
        using var client = factory.CreateClient();

        var departmentResponse = await client.PostAsJsonAsync(
            "/api/v1/adminmasterdata/departments",
            new { name = "Human Resources", description = "People operations", isActive = true });

        departmentResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var departmentId = await ReadDataIdAsync(departmentResponse);

        var positionResponse = await client.PostAsJsonAsync(
            "/api/v1/adminmasterdata/positions",
            new
            {
                departmentId,
                name = "HR Manager",
                description = "Department lead",
                isActive = true,
            });

        var positionBody = await positionResponse.Content.ReadAsStringAsync();
        positionResponse.StatusCode.Should().Be(HttpStatusCode.OK, positionBody);

        var employeeGroupResponse = await client.PostAsJsonAsync(
            "/api/v1/adminmasterdata/employee-groups",
            new { name = "Full Time", description = "Permanent employees", isActive = true });

        employeeGroupResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var departments = await client.GetStringAsync("/api/v1/adminmasterdata/departments");
        departments.Should().Contain("Human Resources");

        var positions = await client.GetStringAsync($"/api/v1/adminmasterdata/positions?departmentId={departmentId}");
        positions.Should().Contain("HR Manager");
        positions.Should().Contain("Human Resources");

        var employeeGroups = await client.GetStringAsync("/api/v1/adminmasterdata/employee-groups");
        employeeGroups.Should().Contain("Full Time");
    }

    [Fact]
    public async Task MasterData_CreateIncompleteRecords_Should_Return_ValidationProblemDetails()
    {
        using var client = factory.CreateClient();

        var departmentResponse = await client.PostAsJsonAsync(
            "/api/v1/adminmasterdata/departments",
            new { name = string.Empty, description = "Missing name", isActive = true });

        var positionResponse = await client.PostAsJsonAsync(
            "/api/v1/adminmasterdata/positions",
            new { departmentId = Guid.Empty, name = string.Empty, isActive = true });

        var employeeGroupResponse = await client.PostAsJsonAsync(
            "/api/v1/adminmasterdata/employee-groups",
            new { name = string.Empty, isActive = true });

        departmentResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        positionResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        employeeGroupResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var departmentBody = await departmentResponse.Content.ReadAsStringAsync();
        var positionBody = await positionResponse.Content.ReadAsStringAsync();
        var employeeGroupBody = await employeeGroupResponse.Content.ReadAsStringAsync();

        departmentBody.Should().Contain("Validation failed");
        departmentBody.Should().Contain("Name");
        positionBody.Should().Contain("DepartmentId");
        positionBody.Should().Contain("Name");
        employeeGroupBody.Should().Contain("Validation failed");
        employeeGroupBody.Should().Contain("Name");
    }

    private static async Task<Guid> ReadDataIdAsync(HttpResponseMessage response)
    {
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);
        return document.RootElement.GetProperty("data").GetProperty("id").GetGuid();
    }
}
