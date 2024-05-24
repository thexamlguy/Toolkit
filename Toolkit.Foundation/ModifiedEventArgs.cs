namespace Toolkit.Foundation;

public record ModifiedEventArgs<TValue>(TValue OldView, TValue NewValue);