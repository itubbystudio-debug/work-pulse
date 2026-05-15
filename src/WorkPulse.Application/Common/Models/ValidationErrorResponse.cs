namespace WorkPulse.Application.Common.Models;

public sealed record ValidationErrorResponse(
    bool Success,
    IReadOnlyDictionary<string, string[]> Errors);
