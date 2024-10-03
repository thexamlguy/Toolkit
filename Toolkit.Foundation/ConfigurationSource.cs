using Microsoft.Extensions.FileProviders;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Toolkit.Foundation;

public class ConfigurationSource<TConfiguration>(IConfigurationFile<TConfiguration> configurationFile,
    string section,
    JsonSerializerOptions? serializerOptions = null) :
    IConfigurationSource<TConfiguration>
    where TConfiguration :
    class
{
    private static readonly Func<JsonSerializerOptions> defaultSerializerOptions = () =>
    {
        return new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingDefault,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Converters =
            {
                new JsonStringEnumConverter(),
                new DictionaryStringObjectJsonConverter()
            }
        };
    };

    public void Set(TConfiguration value) => Set((object)value);

    public void Set(object value)
    {
        using (ConfigurationLock.EnterWrite())
        {
            IFileInfo fileInfo = configurationFile.FileInfo;
            EnsureFileExists(fileInfo.PhysicalPath);

            string? content;
            JsonNode? rootNode;

            using (Stream stream = new FileStream(fileInfo.PhysicalPath!, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            using (StreamReader reader = new(stream))
            {
                content = reader.ReadToEnd();
                stream.Seek(0, SeekOrigin.Begin);

                rootNode = JsonNode.Parse(content);
            }

            JsonNode? valueNode = JsonNode.Parse(JsonSerializer.SerializeToUtf8Bytes(value, serializerOptions ?? defaultSerializerOptions()));

            ApplyConfigurationUpdates(ref rootNode, valueNode, section);

            using Stream stream2 = new FileStream(fileInfo.PhysicalPath!, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite);
            JsonSerializer.Serialize(stream2, rootNode, serializerOptions ?? defaultSerializerOptions());

            ConfigurationCache.Set(section, value);
        }
    }

    public bool TryGet(out TConfiguration? value)
    {
        if (ConfigurationCache.TryGet(section, out value))
        {
            return true;
        }

        using (ConfigurationLock.EnterRead())
        {
            IFileInfo fileInfo = configurationFile.FileInfo;

            if (!File.Exists(fileInfo.PhysicalPath))
            {
                value = default;
                return false;
            }

            using Stream stream = new FileStream(fileInfo.PhysicalPath!, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            using StreamReader reader = new(stream);
            string content = reader.ReadToEnd();
            JsonNode? rootNode = JsonNode.Parse(content);

            string[] segments = section.Split(':');
            JsonNode? currentNode = rootNode;

            int lastIndex = segments.Length - 1;
            for (int i = 0; i < lastIndex; i++)
            {
                if (currentNode is null)
                {
                    value = default;
                    return false;
                }

                currentNode = currentNode[segments[i]];
            }

            if (currentNode != null && currentNode[segments[lastIndex]] is JsonNode sectionNode)
            {
                value = JsonSerializer.Deserialize<TConfiguration>(sectionNode, serializerOptions ?? defaultSerializerOptions());
                ConfigurationCache.Set(section, value);
                return true;
            }

            value = default;
            return false;
        }
    }

    private void ApplyConfigurationUpdates(ref JsonNode? rootNode, JsonNode? valueNode, string section)
    {
        string[] segments = section.Split(':');
        JsonNode? currentNode = rootNode;
        int lastIndex = segments.Length - 1;

        for (int i = 0; i < lastIndex; i++)
        {
            if (currentNode is null)
            {
                return;
            }

            string currentKey = segments[i];
            if (currentNode[currentKey] is null)
            {
                currentNode[currentKey] = new JsonObject();
            }

            currentNode = currentNode[currentKey];
        }

        if (currentNode is not null)
        {
            string lastKey = segments[lastIndex];
            if (currentNode is JsonArray array && int.TryParse(lastKey, out int index))
            {
                if (array.Count <= index)
                {
                    array.Add(valueNode);
                }
                else
                {
                    array[index] = MergeNodes(array[index], valueNode);
                }
            }
            else
            {
                currentNode[lastKey] = MergeNodes(currentNode[lastKey], valueNode);
            }
        }
    }

    private JsonNode? CloneNode(JsonNode? node)
    {
        if (node is null)
        {
            return null;
        }

        string serialized = JsonSerializer.Serialize(node, serializerOptions ?? defaultSerializerOptions());
        return JsonNode.Parse(serialized);
    }

    private void EnsureFileExists(string? filePath)
    {
        if (filePath == null || File.Exists(filePath))
        {
            return;
        }

        string? directoryPath = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        File.WriteAllText(filePath, "{}");
    }

    private JsonNode? MergeNodes(JsonNode? existingNode, JsonNode? newNode)
    {
        if (existingNode is JsonObject existingObject && newNode is JsonObject newObject)
        {
            foreach (KeyValuePair<string, JsonNode?> property in newObject)
            {
                existingObject[property.Key] = MergeNodes(existingObject[property.Key], CloneNode(property.Value));
            }

            return existingObject;
        }
        else if (existingNode is JsonArray existingArray && newNode is JsonArray newArray)
        {
            foreach (JsonNode? item in newArray)
            {
                existingArray.Add(CloneNode(item));
            }

            return existingArray;
        }
        else
        {
            return newNode;
        }
    }
}
