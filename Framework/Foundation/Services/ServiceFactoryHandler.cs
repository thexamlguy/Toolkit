using Mediator;

namespace Toolkit.Framework.Foundation;

public class ServiceFactoryHandler : IRequestHandler<Create, object?>
{
    private readonly IServiceFactory factory;

    public ServiceFactoryHandler(IServiceFactory factory)
    {
        this.factory = factory;
    }
    public async ValueTask<object?> Handle(Create request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(factory.Create(request.Type, request.Parameters));
    }
}
