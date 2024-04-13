namespace Toolkit.Foundation;

public record Insert<TValue>(int Index, TValue Value) : INotification;
