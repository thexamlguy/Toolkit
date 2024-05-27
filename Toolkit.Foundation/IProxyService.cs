namespace Toolkit.Foundation;

public interface IProxyService<TService>
{
    TService Value { get; }
}