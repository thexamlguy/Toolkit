namespace Toolkit.Foundation;

public static class ResultExtensions
{
    public static TResult Failure<TResult>(this object? sender,
        Reason reason)
        where TResult : Result
    {
        if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Result<>))
        {
            object? value = sender is not null && typeof(TResult).GetGenericArguments()[0].IsAssignableFrom(sender.GetType())
                ? sender : Activator.CreateInstance(typeof(TResult).GetGenericArguments()[0].MakeNullable());

            return (TResult)Activator.CreateInstance(typeof(TResult), value, false, reason)!;
        }

        return (TResult)Activator.CreateInstance(typeof(TResult), false, reason)!;
    }

    public static TResult Success<TResult>(this object? sender)
        where TResult : Result
    {
        if (typeof(TResult).IsGenericType && typeof(TResult).GetGenericTypeDefinition() == typeof(Result<>))
        {
            object? value = sender is not null && typeof(TResult).GetGenericArguments()[0].IsAssignableFrom(sender.GetType())
                ? sender : Activator.CreateInstance(typeof(TResult).GetGenericArguments()[0].MakeNullable());

            return (TResult)Activator.CreateInstance(typeof(TResult), value, true, Reason.None)!;
        }

        return (TResult)Activator.CreateInstance(typeof(TResult), true, Reason.None)!;
    }

    public static TResult Create<TResult>(this object? sender, bool condition)
        where TResult : Result => condition ? sender.Success<TResult>() : sender.Failure<TResult>(Reason.ConditionNotMet);
}
