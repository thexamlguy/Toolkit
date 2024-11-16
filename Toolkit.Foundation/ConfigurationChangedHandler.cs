﻿namespace Toolkit.Foundation;

public class ConfigurationChangedHandler<TConfiguration, TValue>(ConfigurationValue<TConfiguration, TValue> configurationValue) :
    IHandler<ChangedEventArgs<TConfiguration>>
    where TValue :
    class, new()
{
    public void Handle(ChangedEventArgs<TConfiguration> args)
    {
        if (args.Sender is TConfiguration configuration)
        {
            if (configurationValue.TryUpdate(configuration, out TValue value))
            {
            }
        }
    }
}