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
}
