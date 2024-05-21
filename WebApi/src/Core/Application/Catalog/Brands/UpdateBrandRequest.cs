namespace FSH.WebApi.Application.Catalog.Brands;

public class UpdateBrandRequest : IRequest<Guid>
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
}

public class UpdateBrandRequestValidator : CustomValidator<UpdateBrandRequest>
{
    public UpdateBrandRequestValidator(IRepository<Brand> repository, IStringLocalizer<UpdateBrandRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (brand, name, ct) =>
                    await repository.FirstOrDefaultAsync(new BrandByNameSpec(name), ct)
                        is not Brand existingBrand || existingBrand.Id == brand.Id)
                .WithMessage((_, name) => T["Brand {0} already Exists.", name]);
}
