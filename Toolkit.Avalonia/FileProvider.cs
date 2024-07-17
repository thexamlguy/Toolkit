using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class FileProvider(ITopLevelProvider topLevelProvider) :
    IFileProvider
{
    public async Task<IReadOnlyCollection<string>> SelectFiles(FileFilter filter)
    {
        if (topLevelProvider.Get() is TopLevel topLevel)
        {
            IReadOnlyList<IStorageFile> storageFiles = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = filter.AllowMultiple,
                FileTypeFilter = new List<FilePickerFileType>
                {
                    new(filter.Name)
                    {
                        Patterns = filter.Extensions.Select(x => $"*.{x}").ToList()
                    }
                }
            });

            return storageFiles.Select(file => file.Path.LocalPath).ToList();
        }

        return Array.Empty<string>();
    }
}