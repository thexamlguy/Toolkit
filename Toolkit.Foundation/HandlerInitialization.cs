using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class HandlerInitialization<TMessage, TResponse, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage, TResponse>
        where TMessage : class
{
    public void Initialize() => WeakReferenceMessenger.Default.Register<IServiceProvider, ResponseEventArgs<TMessage, TResponse>>(provider,
            (provider, args) => args.Reply(provider.GetRequiredService<THandler>().Handle(args.Message)));
}

public class HandlerInitialization<TMessage, THandler>(IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage>
        where TMessage : class
{
    public void Initialize() => WeakReferenceMessenger.Default.Register<IServiceProvider, TMessage>(provider,
        (provider, args) => provider.GetRequiredService<THandler>().Handle(args));
}

public class HandlerKeyedInitialization<TMessage, THandler>(string key, IServiceProvider provider) :
    IInitialization where THandler : class, IHandler<TMessage>
        where TMessage : class
{
    public void Initialize() => WeakReferenceMessenger.Default.Register<IServiceProvider, TMessage, string>(provider, key,
        (provider, args) => provider.GetRequiredKeyedService<THandler>(key).Handle(args));
}