using Json.Patch;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Toolkit.Framework.Foundation;

public class WritableJsonConfigurationBuilder : IWritableJsonConfigurationBuilder
{
    private readonly List<IWritableJsonConfigurationDescriptor> descriptors = new();

    public Stream? DefaultFileStream { get; private set; }

    public IReadOnlyCollection<IWritableJsonConfigurationDescriptor> Descriptors => new ReadOnlyCollection<IWritableJsonConfigurationDescriptor>(descriptors);

    public IWritableJsonConfigurationBuilder AddDefaultConfiguration<TConfiguration>(string Key) where TConfiguration : class
    {
        descriptors.Add(new WritableJsonConfigurationDescriptor(typeof(TConfiguration), Key));
        return this;
    }

    public IWritableJsonConfigurationBuilder AddDefaultFileStream(Stream stream)
    {
        DefaultFileStream = stream;
        return this;
    }

    public void Build(string path)
    {
        JObject? sourceDocument = new();
        if (TryLoadSource(out string? defaultContent))
        {
            sourceDocument = JObject.Parse(defaultContent!);
        }

        JObject? targetDocument = new();
        if (TryLoadTarget(path, out string? targetContent))
        {
            targetDocument = JObject.Parse(targetContent!);
        }

        foreach (IWritableJsonConfigurationDescriptor? descriptor in descriptors)
        {
            if (sourceDocument.SelectToken($"$.{descriptor.Key}") is JToken sourceSection)
            {
                if (targetDocument.SelectToken($"$.{descriptor.Key}") is JToken targetSection)
                {
                    object? source = JsonSerializer.Deserialize(JsonConvert.SerializeObject(sourceSection), descriptor.ConfigurationType);
                    object? target = JsonSerializer.Deserialize(JsonConvert.SerializeObject(targetSection), descriptor.ConfigurationType);

                    JsonPatch? patch = source.CreatePatch(target);

                    object? sourcePatched = patch.Apply(source);
                    targetSection.Replace(JToken.Parse(JsonSerializer.Serialize(sourcePatched, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull })));
                }
                else
                {
                    object? source = JsonSerializer.Deserialize(JsonConvert.SerializeObject(sourceSection), descriptor.ConfigurationType);
                    targetDocument.Add(descriptor.Key, JToken.Parse(JsonSerializer.Serialize(source, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull })));
                }
            }
            else
            {
                object? configuration = Activator.CreateInstance(descriptor.ConfigurationType);
                targetDocument.Add(descriptor.Key, JToken.Parse(JsonSerializer.Serialize(configuration, new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull })));
            }
        }

        using FileStream? fileStream = new(path, FileMode.Create, FileAccess.Write);
        using StreamWriter streamWriter = new(fileStream);
        using JsonTextWriter writer = new(streamWriter) { Formatting = Formatting.Indented };
        targetDocument.WriteTo(writer);
    }

    private bool TryLoadTarget(string path, [MaybeNull] out string? content)
    {
        if (File.Exists(path))
        {
            using FileStream? fileStream = new(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using StreamReader? streamReader = new(fileStream);
            content = streamReader.ReadToEnd();
            return true;
        }

        content = null;
        return false;
    }

    private bool TryLoadSource([MaybeNull] out string? content)
    {
        if (DefaultFileStream is Stream fileStream)
        {
            using StreamReader? streamReader = new(fileStream);
            content = streamReader.ReadToEnd();
            return true;
        }

        content = null;
        return false;
    }
}