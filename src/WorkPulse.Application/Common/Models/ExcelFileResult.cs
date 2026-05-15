namespace WorkPulse.Application.Common.Models;

public sealed record ExcelFileResult(
    string FileName,
    string ContentType,
    byte[] Content);
