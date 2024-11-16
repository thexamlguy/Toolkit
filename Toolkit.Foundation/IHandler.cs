namespace Toolkit.Foundation;

public interface IHandler;

public interface IHandler<TMessage> :
    IHandler
{
    void Handle(TMessage args);
}

public interface IHandler<TMessage, TResponse> :
    IHandler
{
    TResponse Handle(TMessage args);
}