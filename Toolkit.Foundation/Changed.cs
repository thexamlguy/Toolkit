namespace Toolkit.Foundation;

public record Changed<TValue>(TValue? Value = default) : INotification;