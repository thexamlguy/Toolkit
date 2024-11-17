using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Toolkit.Foundation;

public class AsyncResponseEventArgs<TMessage, TResponse> :
    AsyncRequestMessage<TResponse>
{
    public required TMessage Message { get; set; }

    public CancellationToken CancellationToken { get; set; }
}