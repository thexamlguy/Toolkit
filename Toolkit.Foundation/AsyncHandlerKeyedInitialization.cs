using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class AsyncHandlerKeyedInitialization<TMessage, THandler>(string key, 
    IMessenger messenger, 
    IServiceProvider provider) :
    IInitialization, IInitializationScoped where THandler : class, IAsyncHandler<TMessage>
    where TMessage : class
{
    public void Initialize()
    {
        if (!messenger.IsRegistered<AsyncResponseEventArgs<TMessage, Unit>, string>(provider, key))
        {
            messenger.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, Unit>, string>(provider, key,
                (provider, args) =>
                {
                    IEnumerable<IAsyncHandler<TMessage>> handlers = provider.GetKeyedServices<IAsyncHandler<TMessage>>(key);
                    IEnumerable<IAsyncPipelineBehavior<TMessage>> behaviors = provider.GetKeyedServices<IAsyncPipelineBehavior<TMessage>>(key);

                    foreach (IAsyncHandler<TMessage> handler in handlers)
                    {
                        Task<Unit> ExecutePipeline(int index) => index < 0
                            ? handler.Handle(args.Message, args.CancellationToken).ContinueWith(_ => Unit.Value)
                            : behaviors.ElementAt(index).Handle(args.Message, () => ExecutePipeline(index - 1)).ContinueWith(_ => Unit.Value);

                        args.Reply(ExecutePipeline(behaviors.Count() - 1));
                    }
                });
        }
    }
}

public class AsyncHandlerKeyedInitialization<TMessage, TResponse, THandler>(string key, IMessenger messenger, 
    IServiceProvider provider) :
    IInitialization, IInitializationScoped where THandler : class, IAsyncHandler<TMessage, TResponse>
    where TMessage : class
{
    public void Initialize()
    {
        if (!messenger.IsRegistered<AsyncResponseEventArgs<TMessage, TResponse>, string>(provider, key))
        {
            messenger.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, TResponse>, string>(provider, key,
                (provider, args) =>
                {
                    IEnumerable<IAsyncHandler<TMessage, TResponse>> handlers = provider.GetKeyedServices<IAsyncHandler<TMessage, TResponse>>(key);
                    IEnumerable<IAsyncPipelineBehavior<TMessage, TResponse>> behaviors = provider.GetKeyedServices<IAsyncPipelineBehavior<TMessage, TResponse>>(key);

                    foreach (IAsyncHandler<TMessage, TResponse> handler in handlers)
                    {
                        Task<TResponse> ExecutePipeline(int index) => index < 0
                            ? handler.Handle(args.Message, args.CancellationToken)
                            : behaviors.ElementAt(index).Handle(args.Message, () => ExecutePipeline(index - 1));

                        args.Reply(ExecutePipeline(behaviors.Count() - 1));
                    }
                });
        }
    }
}
