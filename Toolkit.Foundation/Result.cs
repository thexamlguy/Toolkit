namespace Toolkit.Foundation;

public record Result<TValue> : Result
{
    public TValue? Value { get; }

    public Result(TValue? value, bool isSuccess, Reason reason)
        : base(isSuccess, reason)
    {
        Value = value;
    }

    public static implicit operator Result<TValue>(TValue? value) =>
        Create(value);

    public static implicit operator TValue?(Result<TValue> result) =>
        result.IsSuccess ? result.Value : default;
}

public record Result
{
    public bool IsSuccess { get; init; }

    public bool IsFailure => !IsSuccess;

    public Reason Reason { get; init; } = Reason.None;

    protected Result(bool isSuccess, Reason reason)
    {
        IsSuccess = isSuccess;
        Reason = reason;
    }

    public static Result Success() => new(true, Reason.None);

    public static Result Failure(Reason reason) => new(false, reason);

    public static Result Create(bool condition) =>
        condition ? Success() : Failure(Reason.ConditionNotMet);

    public static Result<TValue> Success<TValue>(TValue value) =>
        new(value, true, Reason.None);

    public static Result<TValue> Failure<TValue>(Reason reason, TValue? value = default) =>
        new(value, false, reason);

    public static Result<TValue> Create<TValue>(TValue? value) =>
        value is not null ? Success(value) : Failure<TValue>(Reason.Null);

    public static Result Empty() => Failure(Reason.None);

    public static Result<TValue> Empty<TValue>() => Failure<TValue>(Reason.None);
}
