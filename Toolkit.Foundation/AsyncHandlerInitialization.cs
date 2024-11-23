using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class AsyncHandlerInitialization<TMessage, TResponse, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<AsyncResponseEventArgs<TMessage, TResponse>>(provider))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, TResponse>>(provider,
                (provider, args) =>
                {
                    foreach (IAsyncHandler<TMessage, TResponse> handler in provider.GetServices<IAsyncHandler<TMessage, TResponse>>())
                    {
                        args.Reply(handler.Handle(args.Message, args.CancellationToken));
                    }
                });
        }
    }
}

public class AsyncHandlerInitialization<TMessage, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<AsyncResponseEventArgs<TMessage, Unit>>(provider))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, Unit>>(provider,
                (provider, args) =>
                {
                    foreach (IAsyncHandler<TMessage> handler in provider.GetServices<IAsyncHandler<TMessage>>())
                    {
                        handler.Handle(args.Message, args.CancellationToken);
                        args.Reply(Unit.Value);
                    }
                });
        }
    }
}
