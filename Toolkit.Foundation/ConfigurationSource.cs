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
    private readonly object lockingObject = new();

    private static readonly Func<JsonSerializerOptions> defaultSerializerOptions = new(() =>
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
    });

    public void Set(TConfiguration value) => Set((object)value);

    public void Set(object value)
    {
        lock (lockingObject)
        {
            IFileInfo fileInfo = configurationFile.FileInfo;
            if (!File.Exists(fileInfo.PhysicalPath))
            {
                string? fileDirectoryPath = Path.GetDirectoryName(fileInfo.PhysicalPath);
                if (!string.IsNullOrEmpty(fileDirectoryPath))
                {
                    Directory.CreateDirectory(fileDirectoryPath);
                }

                File.WriteAllText(fileInfo.PhysicalPath!, "{}");
            }

            using Stream stream = fileInfo.PhysicalPath is not null
                    ? new FileStream(fileInfo.PhysicalPath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite)
                    : fileInfo.CreateReadStream();

            using StreamReader? reader = new(stream);

            string? content = reader.ReadToEnd();
            stream.Seek(0, SeekOrigin.Begin);

            JsonNode? rootNode = JsonNode.Parse(content);
            JsonNode? valueNode = JsonNode.Parse(JsonSerializer.SerializeToUtf8Bytes(value,
                serializerOptions ?? defaultSerializerOptions()));

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
                        array.Add(value);
                    }
                    else
                    {
                        array[index] = valueNode;
                    }
                }
                else
                {
                    currentNode[lastKey] = valueNode;
                }
            }

            using Stream stream2 = fileInfo.PhysicalPath is not null
                    ? new FileStream(fileInfo.PhysicalPath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)
                    : fileInfo.CreateReadStream();

            JsonSerializer.Serialize(stream, rootNode, serializerOptions ?? defaultSerializerOptions());
        }
    }

    public bool TryGet(out TConfiguration? value)
    {
        lock (lockingObject)
        {
            IFileInfo fileInfo = configurationFile.FileInfo;
            if (File.Exists(fileInfo.PhysicalPath))
            {
                static Stream OpenRead(IFileInfo fileInfo)
                {
                    return fileInfo.PhysicalPath is not null
                        ? new FileStream(fileInfo.PhysicalPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                        : fileInfo.CreateReadStream();
                }

                using Stream stream = OpenRead(fileInfo);
                using StreamReader? reader = new(stream);

                string? content = reader.ReadToEnd();
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

                if (currentNode is not null)
                {
                    if (currentNode[segments[lastIndex]] is JsonNode sectionNode)
                    {
                        value = JsonSerializer.Deserialize<TConfiguration>(sectionNode,
                            serializerOptions ?? defaultSerializerOptions());
                        return true;
                    }
                }
            }

            value = default;
            return false;
        }
    }
}