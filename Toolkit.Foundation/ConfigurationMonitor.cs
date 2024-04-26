namespace Toolkit.Foundation;

public class ConfigurationMonitor<TConfiguration>(IConfigurationFile<TConfiguration> file,
    IConfigurationReader<TConfiguration> reader,
    IPublisher publisher) :
    IConfigurationMonitor<TConfiguration>
    where TConfiguration :
    class
{
    private FileSystemWatcher? watcher;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        async void ChangedHandler(object sender,
            FileSystemEventArgs args)
        {
            if (reader.Read() is { } configuration)
            {
                await publisher.PublishUI(new Changed<TConfiguration>(configuration));
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