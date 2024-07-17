namespace Toolkit.Foundation;

public record FileFilter(string Name, List<string> Extensions, bool AllowMultiple = false);