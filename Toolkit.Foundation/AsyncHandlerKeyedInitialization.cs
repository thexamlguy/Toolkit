using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class AsyncHandlerKeyedInitialization<TMessage, THandler>(string key, IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<TMessage, string>(provider, key))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, TMessage, string>(provider, key,
                (provider, args) =>
                {
                    foreach (IAsyncHandler<TMessage> handler in provider.GetKeyedServices<IAsyncHandler<TMessage>>(key))
                    {
                        handler.Handle(args);
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
        if (!StrongReferenceMessenger.Default.IsRegistered<ResponseEventArgs<TMessage, TResponse>, string>(provider, key))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, ResponseEventArgs<TMessage, TResponse>, string>(provider, key,
                (provider, args) =>
                {
                    foreach (IAsyncHandler<TMessage, TResponse> handler in provider.GetKeyedServices<IAsyncHandler<TMessage, TResponse>>(key))
                    {
                        handler.Handle(args.Message);
                    }
                });
        }
    }
}