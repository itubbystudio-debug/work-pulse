namespace WorkPulse.Application.Common.Models;

public sealed record PagingParameters(int PageNumber = 1, int PageSize = 20)
{
    public int Skip => (Math.Max(PageNumber, 1) - 1) * Math.Clamp(PageSize, 1, 100);

    public int Take => Math.Clamp(PageSize, 1, 100);
}
