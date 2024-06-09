namespace Toolkit.Foundation;

public static class TupleExtensions
{
    public static (T1, T2) CreateValueTuple<T1, T2>(this object[] parameters) => (
            (T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2))
        );

    public static (T1, T2, T3) CreateValueTuple<T1, T2, T3>(this object[] parameters) => (
            (T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2)),
            (T3)Convert.ChangeType(parameters[2], typeof(T3))
        );

    public static (T1, T2, T3, T4) CreateValueTuple<T1, T2, T3, T4>(this object[] parameters) => (
            (T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2)),
            (T3)Convert.ChangeType(parameters[2], typeof(T3)),
            (T4)Convert.ChangeType(parameters[3], typeof(T4))
        );
    public static (T1, T2, T3, T4, T5) CreateValueTuple<T1, T2, T3, T4, T5>(this object[] parameters) => (
            (T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2)),
            (T3)Convert.ChangeType(parameters[2], typeof(T3)),
            (T4)Convert.ChangeType(parameters[3], typeof(T4)),
            (T5)Convert.ChangeType(parameters[4], typeof(T5))
        );

    public static (T1, T2, T3, T4, T5, T6) CreateValueTuple<T1, T2, T3, T4, T5, T6>(this object[] parameters) =>
        ((T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2)),
            (T3)Convert.ChangeType(parameters[2], typeof(T3)),
            (T4)Convert.ChangeType(parameters[3], typeof(T4)),
            (T5)Convert.ChangeType(parameters[4], typeof(T5)),
            (T6)Convert.ChangeType(parameters[5], typeof(T6))
        );

    public static (T1, T2, T3, T4, T5, T6, T7) CreateValueTuple<T1, T2, T3, T4, T5, T6, T7>(this object[] parameters) =>
        ((T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2)),
            (T3)Convert.ChangeType(parameters[2], typeof(T3)),
            (T4)Convert.ChangeType(parameters[3], typeof(T4)),
            (T5)Convert.ChangeType(parameters[4], typeof(T5)),
            (T6)Convert.ChangeType(parameters[5], typeof(T6)),
            (T7)Convert.ChangeType(parameters[6], typeof(T7))
        );

    public static (T1, T2, T3, T4, T5, T6, T7, T8) CreateValueTuple<T1, T2, T3, T4, T5, T6, T7, T8>(this object[] parameters) =>
        ((T1)Convert.ChangeType(parameters[0], typeof(T1)),
            (T2)Convert.ChangeType(parameters[1], typeof(T2)),
            (T3)Convert.ChangeType(parameters[2], typeof(T3)),
            (T4)Convert.ChangeType(parameters[3], typeof(T4)),
            (T5)Convert.ChangeType(parameters[4], typeof(T5)),
            (T6)Convert.ChangeType(parameters[5], typeof(T6)),
            (T7)Convert.ChangeType(parameters[6], typeof(T7)),
            (T8)Convert.ChangeType(parameters[7], typeof(T8))
        );
}
