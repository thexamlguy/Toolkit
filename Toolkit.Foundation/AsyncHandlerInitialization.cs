using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class AsyncHandlerInitialization<TMessage, TResponse, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize() => StrongReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, TResponse>>(provider,
         (provider, args) => args.Reply(provider.GetRequiredService<THandler>().Handle(args.Message, args.CancellationToken)));
                        args.Reply(handler.Handle(args.Message, args.CancellationToken);
}

public class AsyncHandlerInitialization<TMessage, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
{
    public void Initialize() => StrongReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, Unit>>(provider,
         (provider, args) => provider.GetRequiredService<THandler>().Handle(args.Message, args.CancellationToken));
}