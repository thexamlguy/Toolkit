using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IServiceScopeProvider<TService>
{
    bool TryGet(TService service, out IServiceScope? serviceScope);
}