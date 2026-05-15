using ClosedXML.Excel;
using WorkPulse.Application.Common.Interfaces;

namespace WorkPulse.Infrastructure.Services;

public sealed class ExcelReportService : IExcelReportService
{
    public Task<byte[]> CreateWorksheetAsync<T>(
        string worksheetName,
        IReadOnlyCollection<T> rows,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add(worksheetName);
        var properties = typeof(T).GetProperties();

        for (var column = 0; column < properties.Length; column++)
        {
            worksheet.Cell(1, column + 1).Value = properties[column].Name;
        }

        var rowNumber = 2;
        foreach (var row in rows)
        {
            cancellationToken.ThrowIfCancellationRequested();

            for (var column = 0; column < properties.Length; column++)
            {
                worksheet.Cell(rowNumber, column + 1).Value = XLCellValue.FromObject(properties[column].GetValue(row));
            }

            rowNumber++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return Task.FromResult(stream.ToArray());
    }
}
