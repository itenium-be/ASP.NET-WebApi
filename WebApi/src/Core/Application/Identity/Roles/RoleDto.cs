namespace FSH.WebApi.Application.Identity.Roles;

public class RoleDto
{
    public string Id { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public List<string>? Permissions { get; set; }

    public override string ToString() => $"{Id} {Name}. Permissions={string.Join(", ", Permissions ?? new List<string>())}";
}