using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public interface IMessengerRequired
{
    IMessenger Messenger { get; }
}