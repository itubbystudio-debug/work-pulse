namespace WorkPulse.Application.BackendArchitecture.Models;

public sealed record BackendStackResponseDto(
    string Runtime,
    string Architecture,
    string CqrsLibrary,
    string PrimaryDatabase,
    string PrimaryOrm,
    string RawSqlTechnology,
    string ReportingLibrary,
    bool RawSqlConfigured,
    IReadOnlyCollection<BackendStackItemDto> Capabilities);
