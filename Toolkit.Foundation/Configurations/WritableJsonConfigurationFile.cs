using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;

namespace Toolkit.Foundation;

internal class WritableJsonConfigurationFile
{
    private readonly Dictionary<string, (JsonValueKind?, string?)> data = new(StringComparer.OrdinalIgnoreCase);
    private readonly Stack<string> paths = new();
    private JObject tokenCache = new();
    private bool isParsing;

    public IDictionary<string, string?> Parse(Stream input)
    {
        return ParseStream(input);
    }

    public void Write(string key, string value, Stream output)
    {
        if (isParsing)
        {
            return;
        }

        if (key[^1] == ':')
        {
            key = key[0..^1];
        }

        string? tokenPath = $"$.{key.Replace(":", ".")}";
        key = key.Replace("[", "").Replace("]", "");

        if (tokenCache.SelectToken(tokenPath) is JToken token && data.ContainsKey(key))
        {
            (JsonValueKind? kind, string _) = data[key];
            object? newValue = ConvertValue(kind, value);

            data[key] = new(kind, value);
            token.Replace(JToken.FromObject(newValue));
        }

        using StreamWriter streamWriter = new(output);
        using JsonTextWriter writer = new(streamWriter) { Formatting = Formatting.Indented };
        tokenCache.WriteTo(writer);

        output.SetLength(output.Position);
    }

    private static object ConvertValue(JsonValueKind? kind, string value)
    {
        return kind is JsonValueKind.True or JsonValueKind.False ? bool.Parse(value) : value;
    }

    private void EnterContext(string context)
    {
        paths.Push(paths.Count > 0 ? paths.Peek() + ConfigurationPath.KeyDelimiter + context : context);
    }

    private void ExitContext()
    {
        paths.Pop();
    }

    private IDictionary<string, string?> ParseStream(Stream input)
    {
        data.Clear();

        JsonDocumentOptions jsonDocumentOptions = new()
        {
            CommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        isParsing = true;
        using (StreamReader? reader = new(input))
        {
            string? content = reader.ReadToEnd();
            tokenCache = JObject.Parse(content);

            using JsonDocument? doc = JsonDocument.Parse(content, jsonDocumentOptions);
            VisitElement(doc.RootElement);
        }
        isParsing = false;

        return data.ToDictionary(k => k.Key, v => v.Value.Item2?.ToString() ?? null);
    }

    private void VisitElement(JsonElement element)
    {
        bool isEmpty = true;

        foreach (JsonProperty property in element.EnumerateObject())
        {
            isEmpty = false;

            EnterContext(property.Name);
            VisitValue(property.Value);
            ExitContext();
        }

        if (isEmpty && paths.Count > 0)
        {
            data[paths.Peek()] = (JsonValueKind.Null, null);
        }
    }

    private void VisitValue(JsonElement value)
    {
        switch (value.ValueKind)
        {
            case JsonValueKind.Object:
                VisitElement(value);
                break;

            case JsonValueKind.Array:
                int index = 0;
                foreach (JsonElement arrayElement in value.EnumerateArray())
                {
                    EnterContext(index.ToString());
                    VisitValue(arrayElement);
                    ExitContext();
                    index++;
                }
                break;

            case JsonValueKind.Number:
            case JsonValueKind.String:
            case JsonValueKind.True:
            case JsonValueKind.False:
            case JsonValueKind.Null:
                string key = paths.Peek();
                if (data.ContainsKey(key))
                {
                    throw new FormatException();
                }
                data[key] = new(value.ValueKind, value.ToString());
                break;

            case JsonValueKind.Undefined:
                break;

            default:
                throw new FormatException();
        }
    }
}
