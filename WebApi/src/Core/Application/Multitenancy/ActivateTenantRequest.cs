namespace FSH.WebApi.Application.Multitenancy;

public class ActivateTenantRequestValidator : CustomValidator<string>
{
    public ActivateTenantRequestValidator() =>
        RuleFor(t => t)
            .NotEmpty();
}
