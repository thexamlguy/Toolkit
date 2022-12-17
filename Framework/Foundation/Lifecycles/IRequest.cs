namespace Toolkit.Framework.Foundation;

public interface IRequest : IRequest<Unit> { }

public interface IRequest<out TResponse> : IMessage { }
