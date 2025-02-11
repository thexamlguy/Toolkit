namespace Toolkit.Foundation;

public record FilePickerFilter(string Name, List<string> Extensions, bool AllowMultiple = false);
