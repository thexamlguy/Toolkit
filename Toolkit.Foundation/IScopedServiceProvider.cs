namespace Toolkit.Foundation;

public interface IScopedServiceProvider<TService>
{
    bool TryGet(TService service, out IServiceProvider? serviceProvider);
}