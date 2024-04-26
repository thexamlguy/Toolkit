using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class ComponentHost(IServiceProvider services,
    IEnumerable<IInitializer> initializers,
    IEnumerable<IHostedService> hostedServices) :
    IComponentHost
{
    public IServiceProvider Services => services;

    public ComponentConfiguration? Configuration =>
        Services.GetService<ComponentConfiguration>();

    public void Dispose()
    {
    }

    public async Task StartAsync(CancellationToken cancellationToken = default)
    {
        foreach (IInitializer initializer in initializers)
        {
            await initializer.Initialize();
        }

        foreach (IHostedService service in hostedServices)
        {
            await service.StartAsync(cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken = default)
    {
        foreach (IHostedService service in hostedServices)
        {
            await service.StopAsync(cancellationToken);
        }
    }
}