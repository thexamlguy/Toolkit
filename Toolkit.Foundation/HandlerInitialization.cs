using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class HandlerInitialization<TMessage, TResponse, THandler>(IMessenger messenger,
    IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage, TResponse>
    where TMessage : class
{
    public void Initialize()
    {
        if (!messenger.IsRegistered<ResponseEventArgs<TMessage, TResponse>>(provider))
        {
            messenger.Register<IServiceProvider, ResponseEventArgs<TMessage, TResponse>>(provider,
                (provider, args) =>
                {
                    IEnumerable<IHandler<TMessage, TResponse>> handlers = provider.GetServices<IHandler<TMessage, TResponse>>();
                    IEnumerable<IPipelineBehavior<TMessage, TResponse>> behaviors = provider.GetServices<IPipelineBehavior<TMessage, TResponse>>();

                    foreach (IHandler<TMessage, TResponse> handler in handlers)
                    {
                        TResponse ExecutePipeline(int index) => index < 0
                            ? handler.Handle(args.Message)
                            : behaviors.ElementAt(index).Handle(args.Message, () => ExecutePipeline(index - 1));

                        ExecutePipeline(behaviors.Count() - 1);
                    }
                });
        }
    }
}


public class HandlerInitialization<TMessage, THandler>(IMessenger messenger, 
    IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage>
    where TMessage : class
{
    public void Initialize()
    {
        if (!messenger.IsRegistered<TMessage>(provider))
        {
            messenger.Register<IServiceProvider, TMessage>(provider,
                (provider, args) =>
                {
                    IEnumerable<IHandler<TMessage>> handlers = provider.GetServices<IHandler<TMessage>>();
                    IEnumerable<IPipelineBehavior<TMessage>> behaviors = provider.GetServices<IPipelineBehavior<TMessage>>();

                    foreach (IHandler<TMessage> handler in handlers)
                    {
                        void ExecutePipeline(int index)
                        {
                            if (index < 0)
                            {
                                handler.Handle(args);
                                return;
                            }

                            behaviors.ElementAt(index).Handle(args, () =>
                            {
                                ExecutePipeline(index - 1);
                                return Unit.Value;
                            });
                        }

                        ExecutePipeline(behaviors.Count() - 1);
                    }
                });
        }
    }
}
