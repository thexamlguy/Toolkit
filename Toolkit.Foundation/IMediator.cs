
namespace Toolkit.Foundation
{
    public interface IMediator
    {
        Task<object?> Handle(object message, 
            CancellationToken cancellationToken = default);

        Task<TResponse?> Handle<TMessage, TResponse>(TMessage message, 
            CancellationToken cancellationToken = default) 
            where TMessage : notnull;
    }
}