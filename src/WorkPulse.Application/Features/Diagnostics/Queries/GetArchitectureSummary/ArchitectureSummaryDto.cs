namespace WorkPulse.Application.Features.Diagnostics.Queries.GetArchitectureSummary;

public sealed record ArchitectureSummaryDto(
    string Platform,
    string Pattern,
    string Database,
    string PrimaryOrm,
    string RawSql,
    string ExcelReports);
