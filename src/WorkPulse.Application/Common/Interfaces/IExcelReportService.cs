namespace WorkPulse.Application.Common.Interfaces;

public interface IExcelReportService
{
    Task<byte[]> CreateWorksheetAsync<T>(
        string worksheetName,
        IReadOnlyCollection<T> rows,
        CancellationToken cancellationToken);
}
