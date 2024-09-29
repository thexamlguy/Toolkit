using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IServiceScopeFactory<TService>
{
    (IServiceScope, TService)? Create(params object?[] parameters);
}