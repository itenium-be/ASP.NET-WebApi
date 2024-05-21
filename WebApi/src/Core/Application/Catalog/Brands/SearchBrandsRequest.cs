namespace FSH.WebApi.Application.Catalog.Brands;

public class SearchBrandsRequest : PaginationFilter
{
}

public class BrandsBySearchRequestSpec : EntitiesByPaginationFilterSpec<Brand, BrandDto>
{
    public BrandsBySearchRequestSpec(SearchBrandsRequest request)
        : base(request) =>
        Query.OrderBy(c => c.Name, !request.HasOrderBy());
}
