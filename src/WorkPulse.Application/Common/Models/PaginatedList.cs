namespace WorkPulse.Application.Common.Models;

public sealed record PaginatedList<T>(IReadOnlyCollection<T> Items, int PageNumber, int PageSize, int TotalCount)
{
    public int TotalPages => PageSize == 0 ? 0 : (int)Math.Ceiling(TotalCount / (double)PageSize);
}
