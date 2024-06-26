namespace FSH.WebApi.Application.Common.Models;

public class Search
{
    public List<string> Fields { get; set; } = new();
    public string? Keyword { get; set; }

    public override string ToString() => $"{Keyword}, Fields={string.Join(", ", Fields)}";
}