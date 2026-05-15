using MediatR;
using Microsoft.EntityFrameworkCore;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.Features.Diagnostics.Reports.ExportWorkspaces;

public sealed class ExportWorkspacesReportQueryHandler(
    IApplicationDbContext dbContext,
    IExcelReportService excelReportService)
    : IRequestHandler<ExportWorkspacesReportQuery, Result<ExportWorkspacesReportDto>>
{
    public async Task<Result<ExportWorkspacesReportDto>> Handle(
        ExportWorkspacesReportQuery request,
        CancellationToken cancellationToken)
    {
        var rows = await dbContext.Workspaces
            .AsNoTracking()
            .OrderBy(workspace => workspace.Name)
            .Select(workspace => new WorkspaceReportRow(workspace.Id, workspace.Name))
            .ToListAsync(cancellationToken);

        var content = await excelReportService.CreateWorksheetAsync(
            "Workspaces",
            rows,
            cancellationToken);

        return Result<ExportWorkspacesReportDto>.Success(new ExportWorkspacesReportDto(
            "workspaces.xlsx",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            content));
    }
}
