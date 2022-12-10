namespace Toolkit.Framework.Foundation;

public interface IServiceCreator<T>
{
    object Create(Func<Type, object[], object> creator, params object[] parameters);
}