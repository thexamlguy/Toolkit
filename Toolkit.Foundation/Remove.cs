
namespace Toolkit.Foundation;

public record Remove<TValue>(TValue Value) : 
    INotification;