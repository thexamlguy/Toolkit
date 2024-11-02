namespace Toolkit.Windows;

public record PointerDragReleasedEventArgs(PointerLocation Location, PointerButton Button = PointerButton.Left);
