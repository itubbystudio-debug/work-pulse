using MediatR;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;

namespace WorkPulse.Application.BackendArchitecture.Commands.ExportBackendStackReport;

public sealed class ExportBackendStackReportCommandHandler(
    ITechnologyCapabilityRepository repository,
    IExcelReportService excelReportService) : IRequestHandler<ExportBackendStackReportCommand, ExcelFileResult>
{
    public async Task<ExcelFileResult> Handle(ExportBackendStackReportCommand request, CancellationToken cancellationToken)
    {
        var capabilities = await repository.ListAsync(cancellationToken);

        return await excelReportService.CreateBackendStackReportAsync(
            request.ReportTitle!,
            capabilities,
            cancellationToken);
    }
}
