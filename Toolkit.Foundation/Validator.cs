namespace Toolkit.Foundation;

public class Validator(string propertyName,
    ValidationRule[] rules)
{
    private readonly ValidationRule[] rules = rules;

    public string? PropertyName { get; } = propertyName;

    public async Task<(bool isValid, string? message)> TryValidate()
    {
        foreach (ValidationRule rule in rules)
        {
            if (!await rule.ValidateAsync())
            {
                return (false, rule.Message);
            }
        }

        return (true, null);
    }
}
