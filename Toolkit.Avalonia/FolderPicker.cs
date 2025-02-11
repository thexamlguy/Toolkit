using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class FolderPicker(ITopLevelProvider topLevelProvider) :
    IFolderPicker
{
    public async Task<IReadOnlyCollection<string>> Get(FolderPickerPicker filter)
    {
        if (topLevelProvider.Get() is TopLevel topLevel)
        {
            IReadOnlyList<IStorageFolder> folders = await topLevel.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
            {
                AllowMultiple = filter.AllowMultiple  
            });


            return folders.Select(x => x.Path.LocalPath).ToList();
        }

        return [];
    }
}
