using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public class SerialContext<TReader, TValue, TEvent>(IMessenger messenger,
    ISerialConnection connection,
    ISerialStreamer serialStreamer) : 
    ISerialContext<TReader, TValue, TEvent>
    where TReader : SerialReader<TValue>
    where TEvent : SerialEventArgs<TValue>, new()
{
    public bool IsOpen { get; private set; }

    public bool Open()
    {
        if (!connection.Open())
            return false;

        IsOpen = true;

        _ = Task.Run(ReadAsync);
        return true;
    }

    private async Task ReadAsync()
    {
        try
        {
            await using Stream stream = serialStreamer.Create();
            if (Activator.CreateInstance(typeof(TReader), [stream]) is TReader reader)
            {
                await foreach (TValue value in reader.ReadAsync())
                {
                    messenger.Send(new SerialEventArgs<TValue> { Value = value });
                }
            }
        }
        catch
        {
            IsOpen = false;
        }
    }
}
