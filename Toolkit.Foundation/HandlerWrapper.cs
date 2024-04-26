namespace Toolkit.Foundation;

public class HandlerWrapper<TRequest, TResponse>(IHandler<TRequest, TResponse> handler,
    IEnumerable<IPipelineBehaviour<TRequest, TResponse>> pipelineBehaviours)
    where TRequest : class
{
    private readonly IEnumerable<IPipelineBehaviour<TRequest, TResponse>> pipelineBehaviours = 
        pipelineBehaviours.Reverse();

    public async Task<TResponse> Handle(TRequest request, 
        CancellationToken cancellationToken)
    {
        HandlerDelegate<TRequest, TResponse> currentHandler = handler.Handle;
        foreach (IPipelineBehaviour<TRequest, TResponse> behaviour in pipelineBehaviours)
        {
            HandlerDelegate<TRequest, TResponse> previousHandler = currentHandler;
            currentHandler = async (args, token) =>
            {
                return await behaviour.Handle(args, previousHandler, token);
            };
        }

        return await currentHandler(request, cancellationToken);
    }
}