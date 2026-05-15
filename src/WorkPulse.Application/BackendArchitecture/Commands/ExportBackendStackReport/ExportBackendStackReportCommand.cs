using MediatR;
using WorkPulse.Application.Common.Models;
using WorkPulse.Application.Common.Validation;

namespace WorkPulse.Application.BackendArchitecture.Commands.ExportBackendStackReport;

public sealed record ExportBackendStackReportCommand(string? ReportTitle) : IRequest<ExcelFileResult>, IValidatableRequest
{
    public IReadOnlyDictionary<string, string[]> Validate()
    {
        var errors = new Dictionary<string, string[]>(StringComparer.Ordinal);

        if (string.IsNullOrWhiteSpace(ReportTitle))
        {
            errors["reportTitle"] = ["Report title is required."];
        }
        else if (ReportTitle.Length > 120)
        {
            errors["reportTitle"] = ["Report title must be 120 characters or fewer."];
        }

        return errors;
    }
}
