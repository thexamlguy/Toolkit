using Mediator;

namespace Toolkit.Framework.Foundation;

public record Content : IRequest<object?>
{
    public Content(string name, params object?[] parameters)
    {
        Name = name;
        Parameters = parameters;
    }

    public Content(Type type, params object?[] parameters)
    {
        Type = type;
        Parameters = parameters;
    }

    public Type? Type { get; }

    public string? Name { get; }

    public object?[] Parameters { get; }
}