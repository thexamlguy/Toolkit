using Microsoft.Extensions.FileProviders;

namespace Toolkit.Foundation;

public class ConfigurationFile<TConfiguration>(IFileInfo fileInfo) : 
    IConfigurationFile<TConfiguration>
    where TConfiguration :
    class
{
    public IFileInfo FileInfo => fileInfo;
}
