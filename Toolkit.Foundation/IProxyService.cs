namespace Toolkit.Foundation;

public interface IProxyService<TService>
{
    TService Proxy { get; }
}