using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public class AppService(IEnumerable<IInitialization> initializers,
    IPublisher publisher) :
    IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        foreach (IInitialization initializer in initializers)
        {
            initializer.Initialize();
        }

        publisher.Publish<StartedEventArgs>();
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}