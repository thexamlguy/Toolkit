using Mediator;

namespace Toolkit.Framework.Foundation;

public class QueryClassHandlerWrapper<TRequest, TResponse> where TRequest : class, IQuery<TResponse>
{
    private readonly MessageHandlerDelegate<TRequest, TResponse> handler;

    public QueryClassHandlerWrapper(IQueryHandler<TRequest, TResponse> concreteHandler,
        IEnumerable<IPipelineBehavior<TRequest, TResponse>> pipelineBehaviours)
    {
        MessageHandlerDelegate<TRequest, TResponse> handler = concreteHandler.Handle;

        foreach (IPipelineBehavior<TRequest, TResponse>? pipeline in pipelineBehaviours.Reverse())
        {
            MessageHandlerDelegate<TRequest, TResponse> handlerCopy = handler;
            IPipelineBehavior<TRequest, TResponse> pipelineCopy = pipeline;
            handler = (TRequest message, CancellationToken cancellationToken) => pipelineCopy.Handle(message, cancellationToken, handlerCopy);
        }

        this.handler = handler;
    }

    public ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken) =>
        handler(request, cancellationToken);
}
