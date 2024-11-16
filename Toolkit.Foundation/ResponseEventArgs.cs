using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Toolkit.Foundation;

public class ResponseEventArgs<TMessage, TResponse> :
    RequestMessage<TResponse>
{
    public required TMessage Message { get; set; }
}