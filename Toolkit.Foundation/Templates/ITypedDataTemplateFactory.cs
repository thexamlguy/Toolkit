namespace Toolkit.Foundation
{
    public interface ITypedDataTemplateFactory
    {
        object? Create(Type type, params object[] parameters);
    }
}
