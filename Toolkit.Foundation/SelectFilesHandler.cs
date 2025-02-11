namespace Toolkit.Foundation;

public class SelectFilesHandler(IFilePicker fileProvider) :
    IAsyncHandler<SelectionEventArgs<FilePickerFilter>, IReadOnlyCollection<string>?>
{
    public async Task<IReadOnlyCollection<string>?> Handle(SelectionEventArgs<FilePickerFilter> args,
        CancellationToken cancellationToken)
    {
        if (args.Value is FilePickerFilter filter)
        {
            if (await fileProvider.Get(filter)
                is { Count: > 0 } files)
            {
                return files;
            }
        }

        return default;
    }
}
