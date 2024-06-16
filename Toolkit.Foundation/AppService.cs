using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class AppService(IEnumerable<IInitialization> initializers,
    IPublisher publisher) :
    IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (IInitialization initializer in initializers)
        {
            await initializer.OnInitialize();
        }

        publisher.Publish<StartedEventArgs>();
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}