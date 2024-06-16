namespace Toolkit.Foundation;

public class ValidationRule
{
    public ValidationRule(Func<bool> validation,
        string message)
    {
        Validation = validation;
        Message = new Func<string>(() => message);
    }

    public ValidationRule(Func<bool> validation)
    {
        Validation = validation;
    }

    public ValidationRule(Func<bool> validation,
        Func<string> message)
    {
        Validation = validation;
        Message = message;
    }

    public Func<string>? Message { get; }

    public Func<bool>? Validation { get; }
}
