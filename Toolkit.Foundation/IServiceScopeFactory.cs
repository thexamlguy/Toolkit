using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IScopeServiceFactory<TService>
{
    (IServiceScope, TService)? Create(params object?[] parameters);
}