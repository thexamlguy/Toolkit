namespace Toolkit.Foundation;

public class ProxyService<TService>(TService proxy) :
    IProxyService<TService>
{
    public TService Value { get; private set; } = proxy;
}