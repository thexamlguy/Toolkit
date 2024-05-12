﻿namespace Toolkit.Foundation;

public class ConfigurationChangedHandler<TConfiguration, TValue>(ConfigurationValue<TConfiguration, TValue> configurationValue) :
    INotificationHandler<ChangedEventArgs<TConfiguration>>
    where TValue :
    class, new()
{
    public Task Handle(ChangedEventArgs<TConfiguration> args,
        CancellationToken cancellationToken = default)
    {
        if (args.Value is TConfiguration configuration)
        {
            if (configurationValue.TryUpdate(configuration, out TValue value))
            {
            }
        }

        return Task.CompletedTask;
    }
}