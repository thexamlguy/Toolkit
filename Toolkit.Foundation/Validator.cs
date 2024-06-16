using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public class Validator
{
    private readonly ValidationRule[] rules = [];

    public Validator(string propertyName,
        ValidationRule[] rules)
    {
        PropertyName = propertyName;
        this.rules = rules;
    }

    public string? PropertyName { get; }

    public bool TryValidate([MaybeNull] out string message)
    {
        message = "";

        foreach (ValidationRule rule in rules)
        {
            if (rule.Validation?.Invoke() == false)
            {
                message = rule.Message?.Invoke();
                return false;
            }
        }

        return true;
    }
}
