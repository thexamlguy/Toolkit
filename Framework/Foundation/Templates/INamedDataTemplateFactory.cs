namespace Toolkit.Framework.Foundation;

public interface INamedDataTemplateFactory
{
    object? Create(string name, params object[] parameters);
}