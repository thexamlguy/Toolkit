using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class AsyncHandlerKeyedInitialization<TMessage, THandler>(string key, IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<AsyncResponseEventArgs<TMessage, Unit>, string>(provider, key))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, Unit>, string>(provider, key,
                (provider, args) =>
                {
                    foreach (IAsyncHandler<TMessage> handler in provider.GetKeyedServices<IAsyncHandler<TMessage>>(key))
                    {
                        handler.Handle(args.Message, args.CancellationToken);
                        args.Reply(Unit.Value);
                    }
                });
        }
    }
}

public class AsyncHandlerKeyedInitialization<TMessage, TResponse, THandler>(string key, IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<AsyncResponseEventArgs<TMessage, TResponse>, string>(provider, key))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, TResponse>, string>(provider, key,
                (provider, args) =>
                {
                    foreach (IAsyncHandler<TMessage, TResponse> handler in provider.GetKeyedServices<IAsyncHandler<TMessage, TResponse>>(key))
                    {
                        args.Reply(handler.Handle(args.Message, args.CancellationToken));
                    }
                });
        }
    }
}