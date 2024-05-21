namespace Toolkit.Foundation;

public record RemoveAndInsertAtEventArgs<TValue>(int OldIndex, int NewIndex, TValue Value);
