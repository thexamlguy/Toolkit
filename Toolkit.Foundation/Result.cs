﻿namespace Toolkit.Foundation;

public record Result<TValue> :
    Result
{
    private readonly TValue? value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) => this.value = value;

    public TValue? Value => IsSuccess ? value! : default;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}


public record Result(bool IsSuccess, Error Error)
{
    public bool IsFailure => !IsSuccess;

    public static Result Success() => new(true, Error.None);

    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

    public static Result Failure(Error error) => new(false, error);

    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

    public static Result Create(bool condition) => condition ? Success() : Failure(Error.ConditionNotMet);

    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.Null);

}