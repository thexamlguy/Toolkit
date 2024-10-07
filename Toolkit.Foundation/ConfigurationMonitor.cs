using Microsoft.Extensions.DependencyInjection;

namespace Toolkit.Foundation;

public class ConfigurationMonitor<TConfiguration>(string section,
    IConfigurationCache cache,
    IConfigurationFile<TConfiguration> file, 
    IServiceProvider serviceProvider,
    IPublisher publisher) :
    IConfigurationMonitor<TConfiguration>
    where TConfiguration :
    class
{
    private FileSystemWatcher? watcher;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        void ChangedHandler(object sender,
            FileSystemEventArgs args)
        {
            if (serviceProvider.GetRequiredKeyedService<IConfigurationDescriptor<TConfiguration>>(section) is 
                IConfigurationDescriptor<TConfiguration> configuration)
            {
                cache.Remove(section);
                publisher.PublishUI(new ChangedEventArgs<TConfiguration>(configuration.Value));
            }
        }

        if (file.FileInfo.PhysicalPath is { } path)
        {
            string fileName = Path.GetFileName(path);

            watcher = new FileSystemWatcher
            {
                NotifyFilter = NotifyFilters.LastWrite,
                Path = path.Replace(fileName, ""),
                Filter = fileName,
                EnableRaisingEvents = true
            };

            watcher.Changed += ChangedHandler;
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        watcher?.Dispose();
        return Task.CompletedTask;
    }
}