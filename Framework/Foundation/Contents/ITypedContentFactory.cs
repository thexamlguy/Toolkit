namespace Toolkit.Framework.Foundation;

public interface ITypedContentFactory
{
    object? Create(Type type, params object?[] parameters);
}