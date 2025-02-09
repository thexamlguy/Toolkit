using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public class MicroControllerContext<TRead, TEvent>(IReadOnlyCollection<IMicroControllerModuleDescriptor> modules,
    IMessenger messenger,
    ISerialContext serialContext) : 
    IMicroControllerContext<TRead, TEvent> 
    where TEvent : ISerialEventArgs<TRead>
{
    public async Task InitializeAsync()
    {
        //eventAggregator.Subscribe<SerialResponse<TRead>>(OnEvent, null, args => args.Context.Equals(serialContext));
        serialContext.Open();

        await Task.CompletedTask;
    }

    private async void OnEvent(SerialResponse<TRead> args)
    {
        //IMicrocontrollerModule? module = await messenger.SendAsync<IMicrocontrollerModule>(new TReadDeserializer { Read = args.Content }, modules);
        //messenger.Send((dynamic?)module);
    }
}