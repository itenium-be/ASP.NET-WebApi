namespace FSH.WebApi.Application.Catalog.Brands;

public class BrandDto : IDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public BrandType? Type { get; set; }
    public DateOnly? ActiveFrom { get; set; }

    public override string ToString() => $"{Name} - {Description}";
}