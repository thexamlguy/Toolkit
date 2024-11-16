using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class AsyncHandlerInitialization<TMessage, TResponse, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize() => WeakReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, TResponse>>(provider,
        async (provider, args) => args.Reply(await provider.GetRequiredService<THandler>().Handle(args.Message, args.CancellationToken)));
}

public class AsyncHandlerInitialization<TMessage, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IAsyncHandler<TMessage>
        where TMessage : class
{
    public void Initialize() => WeakReferenceMessenger.Default.Register<IServiceProvider, AsyncResponseEventArgs<TMessage, Unit>>(provider,
        async (provider, args) => await provider.GetRequiredService<THandler>().Handle(args.Message, args.CancellationToken));
}