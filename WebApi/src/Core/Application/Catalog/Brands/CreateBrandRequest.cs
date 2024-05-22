namespace FSH.WebApi.Application.Catalog.Brands;

public class CreateBrandRequest
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public override string ToString() => $"{Name} - {Description}";
}

public class CreateBrandRequestValidator : CustomValidator<CreateBrandRequest>
{
    public CreateBrandRequestValidator(IReadRepository<Brand> repository, IStringLocalizer<CreateBrandRequestValidator> T) =>
        RuleFor(p => p.Name)
            .NotEmpty()
            .MaximumLength(75)
            .MustAsync(async (name, ct) => await repository.FirstOrDefaultAsync(new BrandByNameSpec(name), ct) is null)
                .WithMessage((_, name) => T["Brand {0} already Exists.", name]);
}
