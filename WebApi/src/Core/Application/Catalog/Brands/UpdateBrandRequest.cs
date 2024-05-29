namespace FSH.WebApi.Application.Catalog.Brands;

public class UpdateBrandRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public override string ToString() => $"{Name} - {Description}";
}
