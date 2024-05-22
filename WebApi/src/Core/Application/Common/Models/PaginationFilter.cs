namespace FSH.WebApi.Application.Common.Models;

public class PaginationFilter : BaseFilter
{
    public int PageNumber { get; set; }

    public int PageSize { get; set; } = int.MaxValue;

    public string[]? OrderBy { get; set; }

    public override string ToString() => $"Page {PageNumber} (Size={PageSize}), Order={(OrderBy == null ? "None" : string.Join(", ", OrderBy))}";
}

public static class PaginationFilterExtensions
{
    public static bool HasOrderBy(this PaginationFilter filter) =>
        filter.OrderBy?.Any() is true;
}