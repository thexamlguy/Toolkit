﻿using Avalonia.Controls;
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
            IReadOnlyList<IStorageFile> files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                AllowMultiple = filter.AllowMultiple,
                FileTypeFilter =
                [
                    new(filter.Name)
                    {
                        Patterns = filter.Extensions is { Count: > 0 } ? filter.Extensions.Select(x => $"*.{x}").ToList() : ["*.*"]
                    }
                ]
            });

            return files.Select(x => x.Path.LocalPath).ToList();
        }

        return [];
    }
}