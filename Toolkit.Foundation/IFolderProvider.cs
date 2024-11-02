namespace Toolkit.Foundation;

public interface IFolderProvider
{
    Task<IReadOnlyCollection<string>> SelectFolders(FolderFilter filter);
}