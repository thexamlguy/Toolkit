
namespace Toolkit.Framework.Foundation;

public record Navigate : IRequest
{
    public Navigate(string name, params object?[] parameters)
    {
        Name = name;
        Parameters = parameters;
    }

    public Navigate(Type type, params object?[] parameters)
    {
        Type = type;
        Parameters = parameters;
    }

    public Type? Type { get; }

    public object? Route { get; init; }

    public string? Name { get; }

    public object?[] Parameters { get; }
}