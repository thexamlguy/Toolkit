using Microsoft.Extensions.Hosting;

namespace Toolkit.Framework.Foundation;

public class AppService : IHostedService
{
    private readonly IMediator mediator;

    public AppService(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await mediator.Send(new Initialize());
        await mediator.Send(new Initialized());
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}