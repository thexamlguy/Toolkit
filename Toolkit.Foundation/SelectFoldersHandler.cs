namespace Toolkit.Foundation;


public class SelectFoldersHandler(IFolderProvider folderProvider) :
    IAsyncHandler<SelectionEventArgs<FolderFilter>, IReadOnlyCollection<string>?>
{
    public async Task<IReadOnlyCollection<string>?> Handle(SelectionEventArgs<FolderFilter> args,
        CancellationToken cancellationToken = default)
    {
        if (args.Value is FolderFilter filter)
        {
            if (await folderProvider.SelectFolders(filter)
                is { Count: > 0 } folders)
            {
                return folders;
            }
        }

        return default;
    }
}
