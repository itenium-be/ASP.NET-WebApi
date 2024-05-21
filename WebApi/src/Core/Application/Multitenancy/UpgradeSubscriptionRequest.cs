namespace FSH.WebApi.Application.Multitenancy;

public class UpgradeSubscriptionRequest
{
    public string TenantId { get; set; } = default!;
    public DateTime ExtendedExpiryDate { get; set; }
}

public class UpgradeSubscriptionRequestValidator : CustomValidator<UpgradeSubscriptionRequest>
{
    public UpgradeSubscriptionRequestValidator() =>
        RuleFor(t => t.TenantId)
            .NotEmpty();
}
