namespace Toolkit.Foundation;

public class FileProvider :
    IFileProvider
{
    public IReadOnlyCollection<string> Get(string path,
        FileProviderFilter filter)
    {
        if (!Directory.Exists(path))
        {
            return [];
        }

        List<string> searchPatterns = filter.Extensions.Count > 0
            ? filter.Extensions.Select(ext => $"*.{ext}").ToList()
            : ["*.*"];

        List<string> files = [];

        foreach (string pattern in searchPatterns)
        {
            files.AddRange(Directory.EnumerateFiles(path, pattern, SearchOption.TopDirectoryOnly));
        }

        return files;
    }
}
