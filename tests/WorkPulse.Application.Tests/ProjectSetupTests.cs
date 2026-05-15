using WorkPulse.Application.Abstractions.Persistence;
using WorkPulse.Application.Features.ProjectSetup.Commands.InitializeWorkspace;
using WorkPulse.Application.Features.ProjectSetup.Queries.GetProjectBlueprint;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Tests;

public sealed class ProjectSetupTests
{
    [Fact]
    public async Task InitializeWorkspace_ShouldPersistProfile_WhenProjectNameIsValid()
    {
        var repository = new FakeWorkspaceProfileRepository();
        var timeProvider = new FakeTimeProvider(new DateTimeOffset(2026, 5, 15, 12, 0, 0, TimeSpan.Zero));
        var handler = new InitializeWorkspaceCommandHandler(repository, timeProvider);

        var response = await handler.Handle(new InitializeWorkspaceCommand("  Work Pulse  "), CancellationToken.None);

        Assert.Equal("Work Pulse", response.Name);
        Assert.Equal(timeProvider.UtcNow.UtcDateTime, response.CreatedAtUtc);
        Assert.Single(repository.Items);
    }

    [Fact]
    public async Task InitializeWorkspace_ShouldThrow_WhenProjectNameIsBlank()
    {
        var repository = new FakeWorkspaceProfileRepository();
        var handler = new InitializeWorkspaceCommandHandler(
            repository,
            new FakeTimeProvider(new DateTimeOffset(2026, 5, 15, 12, 0, 0, TimeSpan.Zero)));

        await Assert.ThrowsAsync<ArgumentException>(() =>
            handler.Handle(new InitializeWorkspaceCommand("   "), CancellationToken.None));
    }

    [Fact]
    public async Task GetProjectBlueprint_ShouldReturnRequestedStackMetadata()
    {
        var handler = new GetProjectBlueprintQueryHandler();

        var response = await handler.Handle(new GetProjectBlueprintQuery(), CancellationToken.None);

        Assert.Equal("Clean Standard + CQRS + MediatR", response.Architecture);
        Assert.Equal("Entity Framework Core", response.PrimaryOrm);
        Assert.Equal("Dapper", response.SqlQueryTool);
        Assert.Equal(4, response.Layers.Count);
    }

    private sealed class FakeWorkspaceProfileRepository : IWorkspaceProfileRepository
    {
        public List<WorkspaceProfile> Items { get; } = [];

        public Task AddAsync(WorkspaceProfile workspaceProfile, CancellationToken cancellationToken)
        {
            Items.Add(workspaceProfile);
            return Task.CompletedTask;
        }
    }

    private sealed class FakeTimeProvider(DateTimeOffset utcNow) : TimeProvider
    {
        public DateTimeOffset UtcNow { get; } = utcNow;

        public override DateTimeOffset GetUtcNow() => UtcNow;
    }
}
