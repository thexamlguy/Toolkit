using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public static class IMessengerExtensions
{
    public static TResponse Send<TMessage, TResponse>(this IMessenger messenger)
        where TMessage : class, new()
    {
        ResponseEventArgs<TMessage, TResponse> args = messenger.Send(new ResponseEventArgs<TMessage, TResponse> { Message = new TMessage() });
        return args.Response;
    }

    public static TResponse Send<TMessage, TResponse>(this IMessenger messenger,
        TMessage message)
        where TMessage : class
    {
        ResponseEventArgs<TMessage, TResponse> args = messenger.Send(new ResponseEventArgs<TMessage, TResponse> { Message = message });
        return args.Response;
    }

    public static void Send<TMessage>(this IMessenger messenger, 
        TMessage message, string key) where TMessage : class =>
            messenger.Send(message, key);

    public static void Send<TMessage>(this IMessenger messenger,
        string key) where TMessage : class, new() =>
            messenger.Send(new TMessage(), key);

    public static async Task<TResponse> SendAsync<TMessage, TResponse>(this IMessenger messenger)
        where TMessage : class, new() => 
            await messenger.Send(new AsyncResponseEventArgs<TMessage, TResponse> { Message = new TMessage() });

    public static async Task<TResponse> SendAsync<TMessage, TResponse>(this IMessenger messenger,
        TMessage message) where TMessage : class => 
            await messenger.Send(new AsyncResponseEventArgs<TMessage, TResponse> { Message = message });

    public static async Task SendAsync<TMessage>(this IMessenger messenger)
        where TMessage : class, new() =>
            await messenger.Send(new AsyncResponseEventArgs<TMessage, Unit> { Message = new TMessage() });

    public static async Task SendAsync<TMessage>(this IMessenger messenger,
        TMessage message) where TMessage : class =>
            await messenger.Send(new AsyncResponseEventArgs<TMessage, Unit> { Message = message });
}
