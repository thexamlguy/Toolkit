﻿using CommunityToolkit.Mvvm.Messaging;

namespace Toolkit.Foundation;

public class MicrocontrollerContext<TRead, TReadDeserializer>(IReadOnlyCollection<IMicrocontrollerModuleDescriptor> modules,
    IMessenger messenger,
    ISerialContext serialContext) : 
    IMicrocontrollerContext<TRead, TReadDeserializer> where TReadDeserializer : IMicrocontrollerModuleDeserializer<TRead>, new()
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