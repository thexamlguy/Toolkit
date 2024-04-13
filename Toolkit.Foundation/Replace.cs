
namespace Toolkit.Foundation;

public record Replace<TValue>(int Index, TValue Value, object? Target = null) :
    INotification;

