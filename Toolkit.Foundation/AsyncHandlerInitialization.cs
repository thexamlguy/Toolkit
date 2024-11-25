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
                    IEnumerable<IAsyncHandler<TMessage, TResponse>> handlers = provider.GetServices<IAsyncHandler<TMessage, TResponse>>();
                    IEnumerable<IAsyncPipelineBehavior<TMessage, TResponse>> behaviors = provider.GetServices<IAsyncPipelineBehavior<TMessage, TResponse>>();

                    foreach (IAsyncHandler<TMessage, TResponse> handler in handlers)
                    {
                        Task<TResponse> ExecutePipeline(int index) =>index < 0
                            ? handler.Handle(args.Message, args.CancellationToken)
                            : behaviors.ElementAt(index).Handle(args.Message, () => ExecutePipeline(index - 1));

                        args.Reply(ExecutePipeline(behaviors.Count() - 1));
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
                    IEnumerable<IAsyncHandler<TMessage>> handlers = provider.GetServices<IAsyncHandler<TMessage>>();
                    IEnumerable<IAsyncPipelineBehavior<TMessage>> behaviors = provider.GetServices<IAsyncPipelineBehavior<TMessage>>();

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