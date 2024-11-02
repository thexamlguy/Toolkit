namespace Toolkit.Windows;

public record PointerReleasedEventArgs(PointerLocation Location, PointerButton Button = PointerButton.Left);
