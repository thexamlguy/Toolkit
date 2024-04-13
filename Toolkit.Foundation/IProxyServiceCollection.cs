using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public interface IProxyServiceCollection<T>
{
    IServiceCollection Services { get; }
}