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
                async (provider, args) =>
                {
                    IEnumerable<IAsyncHandler<TMessage>> handlers = provider.GetKeyedServices<IAsyncHandler<TMessage>>(key);
                    IEnumerable<IAsyncPipelineBehavior<TMessage, Unit>> behaviors = provider.GetServices<IAsyncPipelineBehavior<TMessage, Unit>>();

                    AsyncHandlerDelegate<Unit> handlerDelegate = async () =>
                    {
                        foreach (IAsyncHandler<TMessage> handler in handlers)
                        {
                            await handler.Handle(args.Message, args.CancellationToken);
                        }
                        return Unit.Value;
                    };

                    foreach (IAsyncPipelineBehavior<TMessage, Unit> behavior in behaviors.Reverse())
                    {
                        AsyncHandlerDelegate<Unit> next = handlerDelegate;
                        handlerDelegate = () => behavior.Handle(args.Message, next);
                    }

                    await handlerDelegate();
                    args.Reply(Unit.Value);
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
                async (provider, args) =>
                {
                    IEnumerable<IAsyncHandler<TMessage, TResponse>> handlers = provider.GetKeyedServices<IAsyncHandler<TMessage, TResponse>>(key);
                    IEnumerable<IAsyncPipelineBehavior<TMessage, TResponse>> behaviors = provider.GetServices<IAsyncPipelineBehavior<TMessage, TResponse>>();

                    AsyncHandlerDelegate<TResponse> handlerDelegate = async () =>
                    {
                        TResponse response = default!;
                        foreach (IAsyncHandler<TMessage, TResponse> handler in handlers)
                        {
                            response = await handler.Handle(args.Message, args.CancellationToken);
                        }
                        return response;
                    };

                    foreach (IAsyncPipelineBehavior<TMessage, TResponse> behavior in behaviors.Reverse())
                    {
                        AsyncHandlerDelegate<TResponse> next = handlerDelegate;
                        handlerDelegate = () => behavior.Handle(args.Message, next);
                    }

                    args.Reply(await handlerDelegate());
                });
        }
    }
}
