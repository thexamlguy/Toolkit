using Mediator;

namespace Toolkit.Framework.Foundation;

public class InitializeHandler : IRequestHandler<Initialize>
{
    private readonly IInitialization initialization;

    public InitializeHandler(IInitialization initialization)
    {
        this.initialization = initialization;
    }

    public async ValueTask<Unit> Handle(Initialize request, CancellationToken cancellationToken)
    {
        await initialization.InitializeAsync();
        return default;
    }
}