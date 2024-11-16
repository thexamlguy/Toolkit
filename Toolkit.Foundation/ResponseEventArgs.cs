using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Toolkit.Foundation;

public class ResponseEventArgs<TMessage, TResponse> :
    RequestMessage<TResponse>
{
    public TMessage? Message { get; set; }
}