namespace Toolkit.Foundation;

public interface IFolderPicker
{
    Task<IReadOnlyCollection<string>> Get(FolderPickerPicker filter);
}