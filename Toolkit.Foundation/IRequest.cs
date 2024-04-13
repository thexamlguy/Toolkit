namespace Toolkit.Foundation;

public interface IRequest<out TResponse> : 
    IMessage;

public interface IRequest : IRequest<Unit>;