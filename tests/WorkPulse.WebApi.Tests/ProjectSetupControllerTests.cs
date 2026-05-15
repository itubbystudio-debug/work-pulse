using System.Net;
using System.Net.Http.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using WorkPulse.Application.Abstractions.Persistence;
using WorkPulse.Domain.Entities;

namespace WorkPulse.WebApi.Tests;

public sealed class ProjectSetupControllerTests
{
    [Fact]
    public async Task GetBlueprint_ShouldReturnRequestedArchitectureMetadata()
    {
        await using var factory = new WorkPulseWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/project-setup/blueprint");

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonObject>();

        Assert.Equal("SQL Server", payload?["database"]?.GetValue<string>());
        Assert.Equal("Dapper", payload?["sqlQueryTool"]?.GetValue<string>());
    }

    [Fact]
    public async Task InitializeWorkspace_ShouldReturnBadRequest_WhenProjectNameIsMissing()
    {
        await using var factory = new WorkPulseWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/project-setup/workspace", new { });

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task InitializeWorkspace_ShouldPersistProfile_WhenProjectNameIsValid()
    {
        await using var factory = new WorkPulseWebApplicationFactory();
        using var client = factory.CreateClient();

        var response = await client.PostAsJsonAsync("/api/project-setup/workspace", new
        {
            projectName = "Work Pulse"
        });

        response.EnsureSuccessStatusCode();

        var payload = await response.Content.ReadFromJsonAsync<JsonObject>();

        Assert.Equal("Work Pulse", payload?["name"]?.GetValue<string>());
    }

    private sealed class WorkPulseWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IWorkspaceProfileRepository>();
                services.AddSingleton<IWorkspaceProfileRepository, FakeWorkspaceProfileRepository>();
            });
        }
    }

    private sealed class FakeWorkspaceProfileRepository : IWorkspaceProfileRepository
    {
        public Task AddAsync(WorkspaceProfile workspaceProfile, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
