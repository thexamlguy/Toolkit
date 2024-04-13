using System.Text.Json.Serialization;

namespace Toolkit.Foundation;

public record ComponentConfiguration
{
    public string? Description { get; set; }

    public string? Name { get; set; }

    [JsonInclude]
    internal Guid Id { get; set; } = Guid.NewGuid();
}