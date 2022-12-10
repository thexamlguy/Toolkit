using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Toolkit.Framework.Foundation;

public class WritableJsonConfigurationProvider : JsonConfigurationProvider, IWritableConfigurationProvider
{
    public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source)
    {
    }

    public void Write<TValue>(string section, TValue value) where TValue : class, new()
    {
        IFileInfo? file = Source.FileProvider?.GetFileInfo(Source.Path ?? string.Empty);
        static Stream OpenRead(IFileInfo fileInfo)
        {
            return fileInfo.PhysicalPath is not null
                ? new FileStream(fileInfo.PhysicalPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite)
                : fileInfo.CreateReadStream();
        }

        if (file is null)
        {
            throw new FileNotFoundException();
        }

        using Stream stream = OpenRead(file);
        using StreamReader? reader = new(stream);

        string? content = reader.ReadToEnd();
        JObject? document = JObject.Parse(content);

        if (document.SelectToken($"$.{section}") is JToken sectionToken)
        {
            sectionToken.Replace(JToken.Parse(JsonConvert.SerializeObject(value)));
            stream.SetLength(0);

            using StreamWriter streamWriter = new(stream);
            using JsonTextWriter writer = new(streamWriter) { Formatting = Formatting.Indented };
            document.WriteTo(writer);
        }
    }
}