
namespace Toolkit.Framework.Foundation;

public record Navigate : IRequest
{
    public Navigate(string path, params object?[] parameters)
    {
        Path = path;
        Parameters = parameters;
    }

    public string? Path { get; }

    public object?[] Parameters { get; }
}