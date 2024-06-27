using Avalonia.Controls;
using Toolkit.Foundation;

namespace Toolkit.Avalonia;

public class FileSelector : 
    IFileSelector
{
    public async Task<IEnumerable<string>> SelectFiles(FileFilter filter)
    {
        //TopLevel topLevel = TopLevel.GetTopLevel(control);

        //var openFileDialog = new OpenFileDialog();
        //openFileDialog.Filters.Add(new FileDialogFilter
        //{
        //    Name = filter.Name,
        //    Extensions = filter.Extensions
        //});

        //openFileDialog.AllowMultiple = filter.AllowMultiple;

        //var results = await openFileDialog.ShowAsync(window as Window);

        //if (results != null && results.Length > 0)
        //{
        //    return results.Select(result => result);
        //}
        return Enumerable.Empty<string>();
    }
}
