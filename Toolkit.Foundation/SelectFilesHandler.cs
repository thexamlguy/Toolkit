namespace Toolkit.Foundation;

public class SelectFilesHandler(IFileProvider fileProvider) :
    IAsyncHandler<SelectionEventArgs<FileFilter>, IReadOnlyCollection<string>?>
{
    public async Task<IReadOnlyCollection<string>?> Handle(SelectionEventArgs<FileFilter> args,
        CancellationToken cancellationToken)
    {
        if (args.Sender is FileFilter filter)
        {
            if (await fileProvider.SelectFiles(filter)
                is { Count: > 0 } files)
            {
                return files;
            }
        }

        return default;
    }
}
