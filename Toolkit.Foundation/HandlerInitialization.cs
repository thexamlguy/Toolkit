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
                    IEnumerable<IHandler<TMessage, TResponse>> handlers = provider.GetServices<IHandler<TMessage, TResponse>>();
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
                    IEnumerable<IHandler<TMessage>> handlers = provider.GetServices<IHandler<TMessage>>();
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

