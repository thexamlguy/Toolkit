namespace Toolkit.Framework.Foundation;

public interface INamedContentFactory
{
    object? Create(string name, params object?[] parameters);
}