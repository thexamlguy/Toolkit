using Microsoft.Extensions.FileProviders;
using System.Text.Encodings.Web;
using System.Text.Json;
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

            static Stream OpenReadWrite(IFileInfo fileInfo)
            {
                return fileInfo.PhysicalPath is not null
                    ? new FileStream(fileInfo.PhysicalPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                    : fileInfo.CreateReadStream();
            }

            using Stream stream = OpenReadWrite(fileInfo);
            using StreamReader? reader = new(stream);

            string? content = reader.ReadToEnd();
            using JsonDocument jsonDocument = JsonDocument.Parse(content);

            using Stream stream2 = OpenReadWrite(fileInfo);
            Utf8JsonWriter writer = new(stream2, new JsonWriterOptions()
            {
                Indented = true,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            writer.WriteStartObject();
            bool isWritten = false;

            JsonDocument optionsElement = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(value,
                serializerOptions ?? defaultSerializerOptions()));

            foreach (JsonProperty element in jsonDocument.RootElement.EnumerateObject())
            {
                if (element.Name != section)
                {
                    element.WriteTo(writer);
                    continue;
                }
                writer.WritePropertyName(element.Name);
                optionsElement.WriteTo(writer);
                isWritten = true;
            }

            if (!isWritten)
            {
                writer.WritePropertyName(section);
                optionsElement.WriteTo(writer);
            }

            writer.WriteEndObject();
            writer.Flush();
            stream2.SetLength(stream2.Position);
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

                using JsonDocument jsonDocument = JsonDocument.Parse(content);
                if (jsonDocument.RootElement.TryGetProperty(section, out JsonElement sectionValue))
                {
                    value = JsonSerializer.Deserialize<TConfiguration>(sectionValue.ToString(), serializerOptions ?? defaultSerializerOptions());
                    return true;
                }
            }

            value = default;
            return false;
        }
    }
}