using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public static class IHostBuilderExtensions
{
    public static IHostBuilder UseContentRoot(this IHostBuilder hostBuilder, 
        string contentRoot,
        bool createDirectory)
    {
        if (createDirectory)
        { 
            Directory.CreateDirectory(contentRoot);
        }

        hostBuilder.UseContentRoot(contentRoot);
        return hostBuilder;
    }
}
