
namespace Toolkit.Foundation
{
    public interface IFileProvider
    {
        IReadOnlyCollection<string> Get(string path, FileProviderFilter filter);
    }
}