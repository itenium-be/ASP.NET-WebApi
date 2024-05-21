namespace FSH.WebApi.Application.Multitenancy;

public class DeactivateTenantRequestValidator : CustomValidator<string>
{
    public DeactivateTenantRequestValidator() =>
        RuleFor(t => t)
            .NotEmpty();
}
