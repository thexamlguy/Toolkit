using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class HandlerInitialization<TMessage, TResponse, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<ResponseEventArgs<TMessage, TResponse>>(provider))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, ResponseEventArgs<TMessage, TResponse>>(provider,
                (provider, args) =>
                {
                    foreach (IHandler<TMessage, TResponse> handler in provider.GetServices<IHandler<TMessage, TResponse>>())
                    {
                        handler.Handle(args.Message);
                    }
                });
        }
    }
}

public class HandlerInitialization<TMessage, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage>
        where TMessage : class
{
    public void Initialize()
    {
        if (!StrongReferenceMessenger.Default.IsRegistered<TMessage>(provider))
        {
            StrongReferenceMessenger.Default.Register<IServiceProvider, TMessage>(provider,
                (provider, args) =>
                {
                    foreach (IHandler<TMessage> handler in provider.GetServices<IHandler<TMessage>>())
                    {
                        handler.Handle(args);
                    }
                });
        }
    }
}
