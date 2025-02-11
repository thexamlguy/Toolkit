using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IScopedServiceProvider<TService>
{
    bool TryGet(TService service, out IServiceScope? serviceScope);
}