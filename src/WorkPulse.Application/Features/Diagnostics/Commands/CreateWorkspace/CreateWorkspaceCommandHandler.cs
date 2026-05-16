using MediatR;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Features.Diagnostics.Commands.CreateWorkspace;

public sealed class CreateWorkspaceCommandHandler(IApplicationDbContext dbContext)
    : IRequestHandler<CreateWorkspaceCommand, Result<CreateWorkspaceResponse>>
{
    public async Task<Result<CreateWorkspaceResponse>> Handle(
        CreateWorkspaceCommand request,
        CancellationToken cancellationToken)
    {
        var workspace = new Workspace(request.Name);

        dbContext.Workspaces.Add(workspace);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateWorkspaceResponse>.Success(new CreateWorkspaceResponse(workspace.Id, workspace.Name));
    }
}
