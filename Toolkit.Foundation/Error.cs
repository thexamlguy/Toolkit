namespace Toolkit.Foundation;

public record Error(string Code, string Message)
{
    public static readonly Error None = new(string.Empty, string.Empty);

    public static readonly Error Null = new("Error.NullValue", "The specified result value is null.");

    public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met.");

    public static readonly Error Duplicated = new("Error.Duplicated", "The specified item already exists.");

    public static readonly Error Failure = new("Error.Failure", "The operation has failed.");
}
