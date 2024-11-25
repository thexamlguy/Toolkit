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
                    IEnumerable<IAsyncHandler<TMessage>> handlers = provider.GetKeyedServices<IAsyncHandler<TMessage>>(key);
                    IEnumerable<IAsyncPipelineBehavior<TMessage, Unit>> behaviors = provider.GetServices<IAsyncPipelineBehavior<TMessage, Unit>>();

                    foreach (IAsyncHandler<TMessage> handler in handlers)
                    {
                        AsyncHandlerDelegate<Unit> handlerDelegate = () =>
                            handler.Handle(args.Message, args.CancellationToken).ContinueWith(_ => Unit.Value);

                        foreach (IAsyncPipelineBehavior<TMessage, Unit> behavior in behaviors.Reverse())
                        {
                            AsyncHandlerDelegate<Unit> next = handlerDelegate;
                            handlerDelegate = () => behavior.Handle(args.Message, next);
                        }

                        handlerDelegate();
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
                    IEnumerable<IAsyncHandler<TMessage, TResponse>> handlers = provider.GetKeyedServices<IAsyncHandler<TMessage, TResponse>>(key);
                    IEnumerable<IAsyncPipelineBehavior<TMessage, TResponse>> behaviors = provider.GetServices<IAsyncPipelineBehavior<TMessage, TResponse>>();

                    foreach (IAsyncHandler<TMessage, TResponse> handler in handlers)
                    {
                        AsyncHandlerDelegate<TResponse> handlerDelegate = () =>
                            handler.Handle(args.Message, args.CancellationToken);

                        foreach (IAsyncPipelineBehavior<TMessage, TResponse> behavior in behaviors.Reverse())
                        {
                            AsyncHandlerDelegate<TResponse> next = handlerDelegate;
                            handlerDelegate = () => behavior.Handle(args.Message, next);
                        }

                        args.Reply(handlerDelegate());
                    }
                });
        }
    }
}