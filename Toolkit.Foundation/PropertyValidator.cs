namespace Toolkit.Foundation;

public class PropertyValidator
{
    public PropertyValidator(Func<bool> validation,
        string message)
    {
        Validation = validation;
        Message = new Func<string>(() => message);
    }

    public PropertyValidator(Func<bool> validation,
        Func<string> message)
    {
        Validation = validation;
        Message = message;
    }

    public Func<string> Message { get; }

    public Func<bool>? Validation { get; }
}
