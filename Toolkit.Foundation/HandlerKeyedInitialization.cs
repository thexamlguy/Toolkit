using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class HandlerKeyedInitialization<TMessage, THandler>(string key,
    IMessenger messenger,
    IServiceProvider provider) :
    IInitialization, IInitializationScoped where THandler : class, IHandler<TMessage>
    where TMessage : class
{
    public void Initialize()
    {
        if (!messenger.IsRegistered<TMessage, string>(provider, key))
        {
            messenger.Register<IServiceProvider, TMessage, string>(provider, key,
                (provider, args) =>
                {
                    IEnumerable<IHandler<TMessage>> handlers = provider.GetKeyedServices<IHandler<TMessage>>(key);
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

public class HandlerKeyedInitialization<TMessage, TResponse, THandler>(string key, 
    IMessenger messenger,
    IServiceProvider provider) :
    IInitialization, IInitializationScoped where THandler : class, IHandler<TMessage, TResponse>
    where TMessage : class
{
    public void Initialize()
    {
        if (!messenger.IsRegistered<ResponseEventArgs<TMessage, TResponse>, string>(provider, key))
        {
            messenger.Register<IServiceProvider, ResponseEventArgs<TMessage, TResponse>, string>(provider, key,
                (provider, args) =>
                {
                    IEnumerable<IHandler<TMessage, TResponse>> handlers = provider.GetKeyedServices<IHandler<TMessage, TResponse>>(key);
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