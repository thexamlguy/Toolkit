using Mediator;
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation
{
    public class AppService : IHostedService
    {
        private readonly IMediator mediator;
        private readonly IInitialization initialization;

        public AppService(IMediator mediator,
            IInitialization initialization)
        {
            this.mediator = mediator;
            this.initialization = initialization;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await mediator.Send(new Initialize());
            await initialization.InitializeAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
