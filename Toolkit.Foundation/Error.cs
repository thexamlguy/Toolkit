namespace Toolkit.Foundation;

public record Reason(string Code, string Message)
{
    public static readonly Reason None = new(string.Empty, string.Empty);

    public static readonly Reason Null = new("Error.NullValue", "The specified result value is null.");

    public static readonly Reason ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met.");

    public static readonly Reason Failure = new("Error.Failure", "The operation has failed.");
}
