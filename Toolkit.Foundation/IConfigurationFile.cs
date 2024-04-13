using Microsoft.Extensions.FileProviders;

namespace Toolkit.Foundation;

public interface IConfigurationFile<TConfiguration>
    where TConfiguration :
    class
{
    IFileInfo FileInfo { get; }
}
