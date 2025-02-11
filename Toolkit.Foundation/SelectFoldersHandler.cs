namespace Toolkit.Foundation;


public class SelectFoldersHandler(IFolderPicker folderProvider) :
    IAsyncHandler<SelectionEventArgs<FolderPickerPicker>, IReadOnlyCollection<string>?>
{
    public async Task<IReadOnlyCollection<string>?> Handle(SelectionEventArgs<FolderPickerPicker> args,
        CancellationToken cancellationToken = default)
    {
        if (args.Value is FolderPickerPicker filter)
        {
            if (await folderProvider.Get(filter)
                is { Count: > 0 } folders)
            {
                return folders;
            }
        }

        return default;
    }
}
