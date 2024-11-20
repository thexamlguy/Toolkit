using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class HandlerKeyedInitialization<TMessage, THandler>(string key, IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<TMessage, string>(provider, key))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, TMessage, string>(provider, key,
                (provider, args) =>
                {
                    foreach (IHandler<TMessage> handler in provider.GetKeyedServices<IHandler<TMessage>>(key))
                    {
                        handler.Handle(args);
                    }
                });
        }
    }
}

public class HandlerKeyedInitialization<TMessage, TResponse, THandler>(string key, IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<ResponseEventArgs<TMessage, TResponse>, string>(provider, key))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, ResponseEventArgs<TMessage, TResponse>, string>(provider, key,
                (provider, args) =>
                {
                    foreach (IHandler<TMessage, TResponse> handler in provider.GetKeyedServices<IHandler<TMessage, TResponse>>(key))
                    {
                        handler.Handle(args.Message);
                    }
                });
        }
    }
}