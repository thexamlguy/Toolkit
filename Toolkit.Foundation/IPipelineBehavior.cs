namespace Toolkit.Foundation;

public interface IPipelineBehavior<TMessage,
    TResponse>
{
    TResponse Handle(TMessage message, 
        HandlerDelegate<TResponse> next);
}

