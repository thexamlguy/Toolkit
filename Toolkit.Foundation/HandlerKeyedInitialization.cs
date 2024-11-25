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
                    IEnumerable<IHandler<TMessage>> handlers = provider.GetKeyedServices<IHandler<TMessage>>(key);
                    IEnumerable<IPipelineBehavior<TMessage, Unit>> behaviors = provider.GetServices<IPipelineBehavior<TMessage, Unit>>();

                    HandlerDelegate<Unit> handlerDelegate = () =>
                    {
                        foreach (IHandler<TMessage> handler in handlers)
                        {
                            handler.Handle(args);
                        }
                        return Unit.Value;
                    };

                    foreach (IPipelineBehavior<TMessage, Unit> behavior in behaviors.Reverse())
                    {
                        HandlerDelegate<Unit> next = handlerDelegate;
                        handlerDelegate = () => behavior.Handle(args, next);
                    }

                    handlerDelegate();
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
                    IEnumerable<IHandler<TMessage, TResponse>> handlers = provider.GetKeyedServices<IHandler<TMessage, TResponse>>(key);
                    IEnumerable<IPipelineBehavior<TMessage, TResponse>> behaviors = provider.GetServices<IPipelineBehavior<TMessage, TResponse>>();

                    HandlerDelegate<TResponse> handlerDelegate = () =>
                    {
                        TResponse response = default!;
                        foreach (IHandler<TMessage, TResponse> handler in handlers)
                        {
                            response = handler.Handle(args.Message);
                        }
                        return response;
                    };

                    foreach (IPipelineBehavior<TMessage, TResponse> behavior in behaviors.Reverse())
                    {
                        HandlerDelegate<TResponse> next = handlerDelegate;
                        handlerDelegate = () => behavior.Handle(args.Message, next);
                    }

                    handlerDelegate();
                });
        }
    }
}
