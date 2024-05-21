namespace Toolkit.Foundation;

public record RemoveEventArgs<TValue>(TValue Value);

public record RemoveAndInsertEventArgs<TValue>(TValue Value);