using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public class Validator
{
    private readonly Action? propertyChanged;
    private readonly PropertyValidator? propertyValidation;

    internal Validator(string propertyName,
        Action propertyChanged)
    {
        PropertyName = propertyName;
        this.propertyChanged = propertyChanged;
    }

    internal Validator(string propertyName,
        Action propertyChanged,
        PropertyValidator validation)
    {
        PropertyName = propertyName;

        this.propertyChanged = propertyChanged;
        propertyValidation = validation;
    }

    internal Validator(string propertyName,
        PropertyValidator validation)
    {
        PropertyName = propertyName;
        propertyValidation = validation;
    }

    public string? PropertyName { get; }

    public void Set() => propertyChanged?.Invoke();

    public bool TryValidate([MaybeNull] out string message)
    {
        message = "";

        if (propertyValidation is not null && propertyValidation.Validation?.Invoke() == false)
        {
            message = propertyValidation.Message.Invoke();
            return false;
        }

        propertyChanged?.Invoke();
        return true;
    }
}
