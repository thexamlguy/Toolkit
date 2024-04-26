
namespace Toolkit.Foundation
{
    public interface IMediator
    {
        Task<object?> Handle(object message, CancellationToken cancellationToken = default);
        Task<TResponse?> Handle<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken = default) where TRequest : notnull;
    }
}