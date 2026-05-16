using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class ShiftApiBehaviorTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateShift_With_Valid_TimeRange_Should_Persist_Shift()
    {
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Morning Shift", startTime = "08:00:00", endTime = "12:00:00" });

        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var shiftId = await ReadDataIdAsync(createResponse);

        var getResponse = await client.GetAsync($"/api/v1/shifts/{shiftId}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await getResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Morning Shift");
        body.Should().Contain("08:00:00");
        body.Should().Contain("12:00:00");
    }

    [Fact]
    public async Task UpdateShift_With_Valid_TimeRange_Should_Persist_Changes()
    {
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Afternoon Shift", startTime = "13:00:00", endTime = "17:00:00" });
        var shiftId = await ReadDataIdAsync(createResponse);

        var updateResponse = await client.PutAsJsonAsync(
            $"/api/v1/shifts/{shiftId}",
            new { name = "Late Afternoon Shift", startTime = "14:00:00", endTime = "18:00:00" });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var getResponse = await client.GetAsync($"/api/v1/shifts/{shiftId}");
        var body = await getResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Late Afternoon Shift");
        body.Should().Contain("14:00:00");
        body.Should().Contain("18:00:00");
    }

    [Fact]
    public async Task CreateShift_With_Invalid_TimeRange_Should_Return_Validation_Problem()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Invalid Shift", startTime = "18:00:00", endTime = "18:00:00" });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Validation failed");
        body.Should().Contain("Shift end time must be after start time.");
    }

    [Fact]
    public async Task CreateShift_With_Overlapping_TimeRange_Should_Return_BadRequest()
    {
        using var client = factory.CreateClient();

        var createResponse = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Overlap Base Shift", startTime = "19:00:00", endTime = "21:00:00" });
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var overlapResponse = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Overlap Candidate Shift", startTime = "20:00:00", endTime = "22:00:00" });

        overlapResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await overlapResponse.Content.ReadAsStringAsync();
        body.Should().Contain("Shift.TimeOverlap");
    }

    [Fact]
    public async Task CreateShift_With_Adjacent_TimeRange_Should_Persist_Shift()
    {
        using var client = factory.CreateClient();

        var firstResponse = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Adjacent First Shift", startTime = "05:00:00", endTime = "06:00:00" });
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var secondResponse = await client.PostAsJsonAsync(
            "/api/v1/shifts",
            new { name = "Adjacent Second Shift", startTime = "06:00:00", endTime = "07:00:00" });

        secondResponse.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private static async Task<Guid> ReadDataIdAsync(HttpResponseMessage response)
    {
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var stream = await response.Content.ReadAsStreamAsync();
        using var document = await JsonDocument.ParseAsync(stream);

        return document.RootElement.GetProperty("data").GetProperty("id").GetGuid();
    }
}
