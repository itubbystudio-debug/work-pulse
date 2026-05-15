using ClosedXML.Excel;
using WorkPulse.Application.Common.Interfaces;
using WorkPulse.Application.Common.Models;
using WorkPulse.Domain.Entities;

namespace WorkPulse.Infrastructure.Reporting;

public sealed class ClosedXmlExcelReportService : IExcelReportService
{
    public Task<ExcelFileResult> CreateBackendStackReportAsync(
        string reportTitle,
        IReadOnlyCollection<TechnologyCapability> capabilities,
        CancellationToken cancellationToken)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Backend Stack");

        worksheet.Cell(1, 1).Value = reportTitle;
        worksheet.Cell(2, 1).Value = "Category";
        worksheet.Cell(2, 2).Value = "Technology";
        worksheet.Cell(2, 3).Value = "Role";

        var row = 3;
        foreach (var capability in capabilities)
        {
            cancellationToken.ThrowIfCancellationRequested();

            worksheet.Cell(row, 1).Value = capability.Category;
            worksheet.Cell(row, 2).Value = capability.Technology;
            worksheet.Cell(row, 3).Value = capability.Role;
            row++;
        }

        worksheet.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);

        var fileName = $"{SanitizeFileName(reportTitle)}.xlsx";
        return Task.FromResult(new ExcelFileResult(
            fileName,
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            stream.ToArray()));
    }

    private static string SanitizeFileName(string value)
    {
        var invalidFileNameChars = Path.GetInvalidFileNameChars();
        var sanitized = new string(value
            .Select(character => invalidFileNameChars.Contains(character) ? '-' : character)
            .ToArray());

        return string.IsNullOrWhiteSpace(sanitized) ? "backend-stack-report" : sanitized.Trim();
    }
}
