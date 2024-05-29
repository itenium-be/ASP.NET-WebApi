namespace FSH.WebApi.Domain.Catalog;

public class Brand : AuditableEntity, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public string Type { get; private set; }
    public DateOnly ActiveFrom { get; private set; }

    public Brand(string name, string? description, string type, DateOnly activeFrom)
    {
        Name = name;
        Description = description;
        ActiveFrom = activeFrom;
        Type = type;
    }

    public Brand Update(string? name, string? description)
    {
        if (name is not null && Name?.Equals(name) is not true) Name = name;
        if (description is not null && Description?.Equals(description) is not true) Description = description;
        return this;
    }

    public override string ToString() => $"{Name} - {Description}";
}