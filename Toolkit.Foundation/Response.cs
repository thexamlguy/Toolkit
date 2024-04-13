namespace Toolkit.Foundation;

public record Response<TValue>(TValue Value) :
    INotification;