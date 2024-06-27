namespace Toolkit.Foundation;

public interface IFileSelector
{
    Task<IEnumerable<string>> SelectFiles(FileFilter filter);
}
