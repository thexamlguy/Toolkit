using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Toolkit.Foundation;

public class DirectoryObserver
{
    public static async Task<string[]> EnumerateFiles(string path,
        string[] patterns,
        int count,
        CancellationToken cancellationToken = default)
    {
        string[] files = [];
        List<Regex> regexPatterns = patterns.Select(pattern => new Regex(pattern, RegexOptions.IgnoreCase)).ToList();

        bool IsBatchComplete()
        {
            files = Directory.EnumerateFiles(path, "*.*", SearchOption.TopDirectoryOnly)
                   .Where(file => regexPatterns.Any(regex => regex.IsMatch(Path.GetFileName(file))))
                   .ToArray();

            if (files.Length != count)
            {
                return false;
            }

            ConcurrentBag<bool> fileAccessResults = [];

            Parallel.ForEach(files, file =>
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
