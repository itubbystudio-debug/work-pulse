using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace WorkPulse.ArchitectureTests;

public sealed class MenuAdministrationTests(CustomWebApplicationFactory factory) : IClassFixture<CustomWebApplicationFactory>
{
    [Fact]
    public async Task CreateMenu_With_Valid_SubMenu_Should_Persist_Hierarchy()
    {
        using var client = factory.CreateClient();

        var parentResponse = await client.PostAsJsonAsync("/api/v1/menus", new
        {
            name = "Administration",
            route = "/admin",
            icon = "settings",
            displayOrder = 1,
            parentMenuId = (Guid?)null,
        });
        parentResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var parentId = await ReadDataIdAsync(parentResponse);

        var childResponse = await client.PostAsJsonAsync("/api/v1/menus", new
        {
            name = "Menus",
            route = "/admin/menus",
            icon = "menu",
            displayOrder = 1,
            parentMenuId = parentId,
        });
        childResponse.StatusCode.Should().Be(
            HttpStatusCode.OK,
            await childResponse.Content.ReadAsStringAsync());
        var childId = await ReadDataIdAsync(childResponse);

        var listResponse = await client.GetAsync("/api/v1/menus");

        listResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        using var document = JsonDocument.Parse(await listResponse.Content.ReadAsStringAsync());
        var root = document.RootElement.GetProperty("data")
            .EnumerateArray()
            .Single(menu => menu.GetProperty("id").GetGuid() == parentId);

        root.GetProperty("name").GetString().Should().Be("Administration");
        var child = root.GetProperty("children")
            .EnumerateArray()
            .Single(menu => menu.GetProperty("id").GetGuid() == childId);
        child.GetProperty("parentMenuId").GetGuid().Should().Be(parentId);
        child.GetProperty("route").GetString().Should().Be("/admin/menus");
    }

    [Fact]
    public async Task UpdateMenu_With_Descendant_As_Parent_Should_Return_BadRequest()
    {
        using var client = factory.CreateClient();

        var parentId = await CreateMenuAsync(client, "Administration", null);
        var childId = await CreateMenuAsync(client, "Menus", parentId);

        var response = await client.PutAsJsonAsync($"/api/v1/menus/{parentId}", new
        {
            name = "Administration",
            route = "/admin",
            icon = "settings",
            displayOrder = 1,
            parentMenuId = childId,
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Menu.InvalidParent");
    }

    [Fact]
    public async Task CreateMenu_With_Missing_Parent_Should_Return_BadRequest()
    {
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/v1/menus", new
        {
            name = "Menus",
            route = "/admin/menus",
            icon = "menu",
            displayOrder = 1,
            parentMenuId = Guid.NewGuid(),
        });

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Menu.ParentNotFound");
    }

    private static async Task<Guid> CreateMenuAsync(HttpClient client, string name, Guid? parentMenuId)
    {
        var response = await client.PostAsJsonAsync("/api/v1/menus", new
        {
            name,
            route = $"/{name.ToLowerInvariant()}",
            icon = "menu",
            displayOrder = 1,
            parentMenuId,
        });
        response.StatusCode.Should().Be(
            HttpStatusCode.OK,
            await response.Content.ReadAsStringAsync());

        return await ReadDataIdAsync(response);
    }

    private static async Task<Guid> ReadDataIdAsync(HttpResponseMessage response)
    {
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return document.RootElement.GetProperty("data").GetProperty("id").GetGuid();
    }
}
