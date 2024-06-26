namespace FSH.WebApi.Application.Multitenancy;

public class CreateTenantRequest
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? ConnectionString { get; set; }
    public string AdminEmail { get; set; } = default!;
    public string? Issuer { get; set; }

    public override string ToString() => $"{Name}, Admin={AdminEmail}";
}
