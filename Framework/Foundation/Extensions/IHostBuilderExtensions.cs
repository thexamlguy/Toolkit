using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public static class IHostBuilderExtensions
{
    public static IHostBuilder UseContentRoot(this IHostBuilder hostBuilder, string contentRoot, bool createDirectory)
    {
        if (!Directory.Exists(contentRoot) && createDirectory)
        {
            Directory.CreateDirectory(contentRoot);
        }

        return hostBuilder.UseContentRoot(contentRoot);
    }
}
