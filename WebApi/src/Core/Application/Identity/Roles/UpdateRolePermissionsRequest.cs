namespace FSH.WebApi.Application.Identity.Roles;

public class UpdateRolePermissionsRequest
{
    public string RoleId { get; set; } = default!;
    public List<string> Permissions { get; set; } = default!;

    public override string ToString() => $"{RoleId}: {string.Join(", ", Permissions)}";
}

public class UpdateRolePermissionsRequestValidator : CustomValidator<UpdateRolePermissionsRequest>
{
    public UpdateRolePermissionsRequestValidator()
    {
        RuleFor(r => r.RoleId)
            .NotEmpty();
        RuleFor(r => r.Permissions)
            .NotNull();
    }
}