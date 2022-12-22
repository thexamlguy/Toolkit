using Microsoft.Extensions.Primitives;
using System.Diagnostics;

namespace Toolkit.Framework.Foundation;

public class NavigationSegment
{
    private NavigationSegment(string name, IDictionary<string, StringValues> parameters, string target)
    {
        Name = name;
        Parameters = parameters;
        Target = target;
    }

    public string Name { get; }

    public IDictionary<string, StringValues> Parameters { get; }

    public string Target { get; }

    public static IReadOnlyCollection<NavigationSegment> FromPath(string? path)
    {
        List<NavigationSegment> result = new();

        if (path is null)
        {
            return result;
        }

        string[] pathParts = path.Split('?');

        string[] segments = pathParts is { Length: >= 1 } ? pathParts[0].Split('/', StringSplitOptions.RemoveEmptyEntries)
            : Array.Empty<string>();
        string query = pathParts is { Length: 2 } ? pathParts[1] : "";

        Dictionary<string, StringValues> parameters = QueryHelpers.ParseQuery(query);

        foreach (string segment in segments)
        {
            string[] segmentParts = segment.Split('#');

            string name = segmentParts is { Length: >= 1 } ? segmentParts[0] : "";
            string target = segmentParts is { Length: 2 } ? segmentParts[1] : "";

            Trace.WriteLine(name);
            Trace.WriteLine(target);

            result.Add(new NavigationSegment(name, parameters, target));
        }

        return result;
    }
}
