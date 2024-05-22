namespace FSH.WebApi.Application.Catalog.Products;

public class SearchProductsRequest : PaginationFilter
{
    public Guid? BrandId { get; set; }
    public decimal? MinimumRate { get; set; }
    public decimal? MaximumRate { get; set; }

    public override string ToString() => $"{MinimumRate} -> {MaximumRate}";
}
