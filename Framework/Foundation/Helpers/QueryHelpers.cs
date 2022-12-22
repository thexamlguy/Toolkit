using Microsoft.Extensions.Primitives;
using System.Text;
using System.Text.Encodings.Web;

namespace Toolkit.Framework.Foundation;

public static class QueryHelpers
{
    public static Dictionary<string, StringValues>? ParseNullableQuery(string? queryString)
    {
        KeyValueAccumulator accumulator = new();
        QueryStringEnumerable enumerable = new(queryString);

        foreach (QueryStringEnumerable.EncodedNameValuePair pair in enumerable)
        {
            accumulator.Append(pair.DecodeName().ToString(), pair.DecodeValue().ToString());
        }

        if (!accumulator.HasValues)
        {
            return null;
        }

        return accumulator.GetResults();
    }

    public static Dictionary<string, StringValues> ParseQuery(string? queryString)
    {
        Dictionary<string, StringValues>? result = ParseNullableQuery(queryString);
        if (result == null)
        {
            return new Dictionary<string, StringValues>();
        }

        return result;
    }
}