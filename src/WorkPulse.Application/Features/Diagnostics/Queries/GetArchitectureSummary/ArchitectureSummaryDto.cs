namespace WorkPulse.Application.Features.Diagnostics.Queries.GetArchitectureSummary;

public sealed record ArchitectureSummaryDto(
    string Platform,
    string Pattern,
    string Database,
    string PrimaryOrm,
    string RawSql,
    string ExcelReports,
    FrontendStackPolicyDto Frontend);

public sealed record FrontendStackPolicyDto(
    string Framework,
    string UiLibrary,
    string Template,
    string Scope,
    string VersionPolicy,
    IReadOnlyCollection<string> RequiredComponents,
    string ExceptionPolicy,
    string LegacyPolicy);
