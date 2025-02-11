namespace Toolkit.Foundation;

public interface IFilePicker
{
    Task<IReadOnlyCollection<string>> Get(FilePickerFilter filter);
}