namespace Toolkit.Foundation;

public interface IPipelineBehavior<TMessage, 
    TResponse>
{
    TResponse Handle(TMessage message,
        Func<TResponse> next);
}

public interface IPipelineBehavior<TMessage>
{
    void Handle(TMessage message,
        Func<Unit> next);
}