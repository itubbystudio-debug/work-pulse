using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class OrganizationStructureApiTests(CustomWebApplicationFactory factory)
    : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateOrganizationHierarchy_Should_Persist_Nodes()
    {
        using var client = factory.CreateClient();

        var rootId = await CreateNodeAsync(client, "Head Office", null);
        var childId = await CreateNodeAsync(client, "Finance", rootId);

        var response = await client.GetAsync("/api/v1/organizationstructure/nodes");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain(rootId.ToString());
        body.Should().Contain(childId.ToString());
        body.Should().Contain("Head Office");
        body.Should().Contain("Finance");
        body.Should().Contain(rootId.ToString());
    }

    [Fact]
    public async Task UpdateOrganizationNode_With_ParentChildLoop_Should_Return_ProblemDetails()
    {
        using var client = factory.CreateClient();
        var rootId = await CreateNodeAsync(client, "Head Office", null);
        var childId = await CreateNodeAsync(client, "Finance", rootId);

        var response = await client.PutAsJsonAsync(
            $"/api/v1/organizationstructure/nodes/{rootId}",
            new { name = "Head Office", parentId = childId });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/problem+json");
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("OrganizationNode.InvalidHierarchy");
        body.Should().Contain("parent-child loop");
    }

    [Fact]
    public async Task UpdateOrganizationNode_With_Valid_Parent_Should_Persist_Change()
    {
        using var client = factory.CreateClient();
        var rootId = await CreateNodeAsync(client, "Head Office", null);
        var childId = await CreateNodeAsync(client, "Finance", rootId);

        var updateResponse = await client.PutAsJsonAsync(
            $"/api/v1/organizationstructure/nodes/{childId}",
            new { name = "Finance and Accounting", parentId = (Guid?)null });

        updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var response = await client.GetAsync("/api/v1/organizationstructure/nodes");
        var body = await response.Content.ReadAsStringAsync();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        body.Should().Contain(childId.ToString());
        body.Should().Contain("Finance and Accounting");
    }

    private static async Task<Guid> CreateNodeAsync(HttpClient client, string name, Guid? parentId)
    {
        var response = await client.PostAsJsonAsync(
            "/api/v1/organizationstructure/nodes",
            new { name, parentId });

        var responseBody = await response.Content.ReadAsStringAsync();
        response.StatusCode.Should().Be(HttpStatusCode.OK, responseBody);

        using var document = JsonDocument.Parse(responseBody);

        return document.RootElement
            .GetProperty("data")
            .GetProperty("id")
            .GetGuid();
    }
}
