namespace Toolkit.Framework.Foundation;

public class RequestClassHandlerWrapper<TRequest, TResponse> where TRequest : class, IRequest<TResponse>
{
    private readonly MessageHandlerDelegate<TRequest, TResponse> handler;

    public RequestClassHandlerWrapper(IRequestHandler<TRequest, TResponse> concreteHandler,
        IEnumerable<IPipelineBehavior<TRequest, TResponse>> pipelineBehaviours)
    {
        MessageHandlerDelegate<TRequest, TResponse> handler = concreteHandler.Handle;
        foreach (var pipeline in pipelineBehaviours.Reverse())
        {
            MessageHandlerDelegate<TRequest, TResponse> handlerCopy = handler;
            IPipelineBehavior<TRequest, TResponse> pipelineCopy = pipeline;

            handler = (TRequest message, CancellationToken cancellationToken) => pipelineCopy.Handle(message, cancellationToken, handlerCopy);
        }

        this.handler = handler;
    }

    public ValueTask<TResponse> Handle(TRequest request, CancellationToken cancellationToken) => handler(request, cancellationToken);
}