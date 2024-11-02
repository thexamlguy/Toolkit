using System.ComponentModel;
using System.Linq.Expressions;

namespace Toolkit.Foundation;

public class Validation(IValidatorCollection validators) :
    IValidation
{
    private readonly ValidationErrorCollection errors = [];

    public event PropertyChangedEventHandler? PropertyChanged;

    public IReadOnlyIndexDictionary<string, string> Errors => 
        new ReadOnlyIndexDictionary<string, string>(errors);

    public bool HasErrors =>
        Errors.Count > 0;

    internal IValidatorCollection Validators { get; } = validators;

    public void Add<TProperty>(Expression<Func<TProperty>> property,
        ValidationRule[] rules,
        ValidationTrigger trigger = ValidationTrigger.Deferred)
    {
        string? name = GetPropertyName(property);
        Validators.Add(name, new Validator(name, rules));

        if (trigger is ValidationTrigger.Immediate)
        {
            _ = Validate(name);
        }
    }

    public void Clear()
    {
        errors.Clear();
        OnPropertyChanged(nameof(Errors), null, null);
    }

    public virtual void OnPropertyChanged(string propertyName,
            object? before, object? after)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public async Task<bool> Validate<TProperty>(Expression<Func<TProperty>> property,
        ValidationRule[] rules)
    {
        string? name = GetPropertyName(property);
        Validator validator = new(name, rules);

        Clear(name);

        (bool isValid, string? message) = await validator.TryValidate();

        if (!isValid)
        {
            errors[name] = message ?? "";
        }

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);

        return !HasErrors;
    }

    public async Task<bool> Validate(string name)
    {
        Clear(name);

        if (Validators.TryGet(name, out Validator? validator))
        {
            if (validator is not null)
            {
                (bool isValid, string? message) = await validator.TryValidate();
                if (!isValid)
                {
                    errors[name] = message ?? "";
                }
            }
        }

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);

        return !HasErrors;
    }

    public async Task<bool> Validate()
    {
        Clear();

        foreach (Validator? validator in Validators)
        {
            if (validator.PropertyName is string name)
            {
                (bool isValid, string? message) = await validator.TryValidate();
                if (!isValid)
                {
                    errors[name] = message ?? "";
                }
            }
        }

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);

        return !HasErrors;
    }

    private void Clear(string name)
    {
        if (Errors.ContainsKey(name))
        {
            errors.Remove(name);
            OnPropertyChanged(nameof(Errors), null, null);
        }
    }

    private string GetPropertyName<T>(Expression<Func<T>> expression)
    {
        return expression.Body switch
        {
            MemberExpression memberExpression => memberExpression.Member.Name,
            UnaryExpression unaryExpression when unaryExpression.Operand is MemberExpression operand => operand.Member.Name,
            _ => string.Empty
        };
    }
}