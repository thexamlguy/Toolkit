using System.Collections.Concurrent;

namespace Toolkit.Foundation;

public class DirectoryObserver
{
    public static async Task<string[]> EnumerateFiles(string path,
        string[] filter,
        int count,
        CancellationToken cancellationToken = default)
    {
        string[] files = [];
        HashSet<string> extensions = filter.Select(x => $".{x.ToLower()}").ToHashSet();

        bool IsBatchComplete()
        {
            files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                   .Where(x => extensions.Contains(Path.GetExtension(x).ToLower()))
                   .ToArray();

            if (files.Length != count)
            {
                return false;
            }

            ConcurrentBag<bool> fileAccessResults = [];
            Parallel.ForEach(files, (file) =>
            {
                try
                {
                    using FileStream fileStream = new(file, FileMode.Open, FileAccess.Read, FileShare.None);
                    fileAccessResults.Add(true);
                }
                catch (IOException)
                {
                    fileAccessResults.Add(false);
                }
            });

            return fileAccessResults.All(result => result);
        }

        await Task.Run(async () =>
        {
            while (!IsBatchComplete())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Delay(5000, cancellationToken);
            }
        }, cancellationToken);

        return files;
    }
}
