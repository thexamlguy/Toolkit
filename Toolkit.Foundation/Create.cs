
namespace Toolkit.Foundation;

public record Create<TValue>(TValue Value) :
    INotification;