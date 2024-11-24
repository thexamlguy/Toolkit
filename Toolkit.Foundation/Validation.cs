using System.ComponentModel;
using System.Linq.Expressions;

namespace Toolkit.Foundation;

public class Validation(IValidatorCollection validators) : IValidation
{
    private readonly ValidationErrorCollection errors = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public IReadOnlyIndexDictionary<string, string> Errors =>
        new ReadOnlyIndexDictionary<string, string>(errors);

    public bool HasErrors => Errors.Count > 0;

    internal IValidatorCollection Validators { get; } = validators;

    public void Add<TProperty>(Expression<Func<TProperty>> property, ValidationRule[] rules, ValidationTrigger trigger = ValidationTrigger.Deferred)
    {
        string name = GetPropertyName(property);
        Validators.Add(name, new Validator(name, rules));
        if (trigger == ValidationTrigger.Immediate) _ = Validate(name);
    }

    public void Clear()
    {
        errors.Clear();
        OnPropertyChanged(nameof(Errors), null, null);
    }

    public Task<bool> Validate<TProperty>(Expression<Func<TProperty>> property, ValidationRule[] rules) =>
        ValidateInternal(new Validator(GetPropertyName(property), rules), GetPropertyName(property));

    public Task<bool> Validate(string name, ValidationRule[] rules) =>
        ValidateInternal(new Validator(name, rules), name);

    public Task<bool> Validate(string name) =>
        Validators.TryGet(name, out var validator) && validator != null
            ? ValidateInternal(validator, name)
            : Task.FromResult(true);

    public async Task<bool> Validate()
    {
        Clear();

        foreach (var validator in Validators)
        {
            if (validator.PropertyName is string name)
            {
                (bool isValid, string? message) = await validator.TryValidate();
                if (!isValid) errors[name] = message ?? string.Empty;
            }
        }

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);
        return !HasErrors;
    }

    private async Task<bool> ValidateInternal(Validator validator, string name)
    {
        Clear(name);

        (bool isValid, string? message) = await validator.TryValidate();
        if (!isValid) errors[name] = message ?? string.Empty;

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);
        return isValid;
    }

    private void Clear(string name)
    {
        if (errors is not null && errors.ContainsKey(name))
        {
            errors.Remove(name);
            OnPropertyChanged(nameof(Errors), null, null);
        }
    }

    private static string GetPropertyName<T>(Expression<Func<T>> expression) =>
        expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression unaryExpression when unaryExpression.Operand is MemberExpression operand => operand.Member.Name,
            _ => string.Empty
        };

    public virtual void OnPropertyChanged(string propertyName, object? before, object? after) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
}
