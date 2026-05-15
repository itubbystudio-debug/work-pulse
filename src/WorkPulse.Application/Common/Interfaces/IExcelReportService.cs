using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Application.Common.Interfaces;

public interface IExcelReportService
{
    Task<ExcelFileResult> CreateBackendStackReportAsync(
        string reportTitle,
        IReadOnlyCollection<TechnologyCapability> capabilities,
        CancellationToken cancellationToken);
}
