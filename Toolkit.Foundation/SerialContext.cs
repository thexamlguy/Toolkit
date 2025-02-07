using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public class SerialContext<TSerialReader, TContent>(IMessenger messenger,
    ISerialConnection connection,
    ISerialStreamer serialStreamer) : 
    ISerialContext<TSerialReader, TContent> where TSerialReader : SerialReader<TContent>
{
    public async void Open()
    {
        if (connection.Open())
        {
            Stream stream = serialStreamer.Create();

            if ((TSerialReader?)Activator.CreateInstance(typeof(TSerialReader), [stream]) is TSerialReader reader)
            {
                await foreach (TContent content in reader.ReadAsync())
                {
                    messenger.Send(SerialResponse.Create(this, content));
                }
            }
        }
    }
}
