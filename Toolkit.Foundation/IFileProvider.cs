namespace Toolkit.Foundation;

public interface IFileProvider
{
    Task<IReadOnlyCollection<string>> SelectFiles(FileFilter filter);
}