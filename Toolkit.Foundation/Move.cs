namespace Toolkit.Foundation;

public record Move<TValue>(int Index, TValue Value) : INotification;