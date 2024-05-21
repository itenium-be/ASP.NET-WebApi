using FSH.WebApi.Domain.Common.Events;

namespace FSH.WebApi.Application.Catalog.Products;

public class CreateProductRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal Rate { get; set; }
    public Guid BrandId { get; set; }
    public FileUploadRequest? Image { get; set; }
}
