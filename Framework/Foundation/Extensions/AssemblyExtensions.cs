using System.Reflection;

namespace Toolkit.Framework.Foundation;

public static class AssemblyExtensions
{
    public static Stream? ExtractResource(this Assembly assembly, string filename)
    {
        string? resourceName = $"{assembly.GetName()?.Name?.Replace("-", "_")}.{filename}";
        return assembly.GetManifestResourceStream(resourceName);
    }
}
