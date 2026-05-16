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
    public async Task CreateCompanyHoliday_With_Valid_Input_Should_Persist()
    {
        using var client = factory.CreateClient();
        var holidayDate = new DateOnly(2026, 4, 13);

        var createResponse = await client.PostAsJsonAsync(
            "/api/v1/companyholidays",
            new { date = holidayDate, name = "Songkran Festival" });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createBody = await createResponse.Content.ReadAsStringAsync();
        createBody.Should().Contain("Songkran Festival");
        createBody.Should().Contain("2026-04-13");

        var listResponse = await client.GetAsync("/api/v1/companyholidays?from=2026-04-01&to=2026-04-30");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var listBody = await listResponse.Content.ReadAsStringAsync();
        listBody.Should().Contain("Songkran Festival");
        listBody.Should().Contain("2026-04-13");
    }

    [Fact]
    public async Task UpdateCompanyHoliday_With_Valid_Input_Should_Save_Changes()
    {
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync(
            "/api/v1/companyholidays",
            new { date = new DateOnly(2026, 5, 1), name = "Labor Day" });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var created = await createResponse.Content.ReadFromJsonAsync<ApiEnvelope<CompanyHolidayResponse>>();
        created?.Data.Should().NotBeNull();

        var updateResponse = await client.PutAsJsonAsync(
            $"/api/v1/companyholidays/{created!.Data!.Id}",
            new { date = new DateOnly(2026, 5, 4), name = "Substitution Holiday" });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await client.GetAsync($"/api/v1/companyholidays/{created.Data.Id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Substitution Holiday");
        body.Should().Contain("2026-05-04");
    }

    [Fact]
    public async Task CreateCompanyHoliday_With_Invalid_Date_Should_Return_ProblemDetails()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/v1/companyholidays",
            new { date = default(DateOnly), name = "Invalid Holiday" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Validation failed");
        body.Should().Contain("Date");
    }

    [Fact]
    public async Task CreateCompanyHoliday_With_Duplicate_Date_Should_Return_ProblemDetails()
    {
        using var client = factory.CreateClient();
        var holidayDate = new DateOnly(2026, 12, 5);

        var firstResponse = await client.PostAsJsonAsync(
            "/api/v1/companyholidays",
            new { date = holidayDate, name = "Father's Day" });
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var duplicateResponse = await client.PostAsJsonAsync(
            "/api/v1/companyholidays",
            new { date = holidayDate, name = "Duplicate Holiday" });

        duplicateResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        duplicateResponse.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await duplicateResponse.Content.ReadAsStringAsync();
        body.Should().Contain("A company holiday already exists for this date.");
    }
}

file sealed record ApiEnvelope<T>(bool Success, T? Data);

file sealed record CompanyHolidayResponse(Guid Id, DateOnly Date, string Name);
