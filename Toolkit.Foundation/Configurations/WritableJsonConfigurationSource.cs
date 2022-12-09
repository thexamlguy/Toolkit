using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace Toolkit.Foundation;

public class WritableJsonConfigurationSource : JsonConfigurationSource
{
    public IWritableJsonConfigurationBuilder? Factory { get; set; }

    public override IConfigurationProvider Build(IConfigurationBuilder builder)
    {
        EnsureDefaultsWithSteam(builder);
        return new WritableJsonConfigurationProvider(this);
    }

    private void EnsureDefaultsWithSteam(IConfigurationBuilder builder)
    {
        EnsureDefaults(builder);

        if (FileProvider is PhysicalFileProvider physicalFileProvider)
        {
            string? outputFile = System.IO.Path.Combine(physicalFileProvider.Root, Path);
            Factory?.Build(outputFile);
        }
    }
}
