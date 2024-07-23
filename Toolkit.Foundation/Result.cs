namespace Toolkit.Foundation;

public record Result<TValue> :
    Result
{
    private readonly TValue? value;

    protected internal Result(TValue? value, bool isSuccess, Error error)
        : base(isSuccess, error) => this.value = value;

    public TValue? Value => IsSuccess ? value! : default;

    public static implicit operator Result<TValue>(TValue? value) => Create(value);
}
