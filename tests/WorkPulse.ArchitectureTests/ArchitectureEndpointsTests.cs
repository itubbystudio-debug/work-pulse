using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using ClosedXML.Excel;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class ArchitectureEndpointsTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task GetStack_ReturnsConfiguredBackendArchitecture()
    {
        var response = await _client.GetAsync("/api/architecture/stack");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        using var contentStream = await response.Content.ReadAsStreamAsync();
        using var payload = await JsonDocument.ParseAsync(contentStream);

        payload.RootElement.GetProperty("success").GetBoolean().Should().BeTrue();

        var data = payload.RootElement.GetProperty("data");
        data.GetProperty("runtime").GetString().Should().Be(".NET 10 Web API");
        data.GetProperty("architecture").GetString().Should().Be("Clean Architecture");
        data.GetProperty("cqrsLibrary").GetString().Should().Be("MediatR");
        data.GetProperty("primaryDatabase").GetString().Should().Be("SQL Server");
        data.GetProperty("primaryOrm").GetString().Should().Be("EF Core");
        data.GetProperty("rawSqlTechnology").GetString().Should().Be("Dapper");
        data.GetProperty("reportingLibrary").GetString().Should().Be("ClosedXML");
        data.GetProperty("capabilities").GetArrayLength().Should().BeGreaterThanOrEqualTo(5);
    }

    [Fact]
    public async Task ExportReport_WithInvalidInput_ReturnsBadRequest()
    {
        var response = await _client.PostAsJsonAsync("/api/architecture/report", new
        {
            reportTitle = ""
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        using var contentStream = await response.Content.ReadAsStreamAsync();
        using var payload = await JsonDocument.ParseAsync(contentStream);

        payload.RootElement.GetProperty("success").GetBoolean().Should().BeFalse();
        payload.RootElement.GetProperty("errors").GetProperty("reportTitle")[0].GetString()
            .Should().Be("Report title is required.");
    }

    [Fact]
    public async Task ExportReport_ReturnsExcelWorkbook()
    {
        var response = await _client.PostAsJsonAsync("/api/architecture/report", new
        {
            reportTitle = "Backend Architecture Baseline"
        });

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType
            .Should().Be("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        var bytes = await response.Content.ReadAsByteArrayAsync();
        await using var stream = new MemoryStream(bytes);
        using var workbook = new XLWorkbook(stream);
        var worksheet = workbook.Worksheet("Backend Stack");

        worksheet.Cell(1, 1).GetString().Should().Be("Backend Architecture Baseline");
        worksheet.Cell(2, 1).GetString().Should().Be("Category");
        worksheet.LastRowUsed().Should().NotBeNull();
        worksheet.LastRowUsed()!.RowNumber().Should().BeGreaterThan(2);
    }
}
