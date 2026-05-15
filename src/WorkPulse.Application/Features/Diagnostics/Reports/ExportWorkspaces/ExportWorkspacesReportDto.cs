namespace WorkPulse.Application.Features.Diagnostics.Reports.ExportWorkspaces;

public sealed record ExportWorkspacesReportDto(string FileName, string ContentType, byte[] Content);
