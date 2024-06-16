using System.ComponentModel;
using System.Linq.Expressions;

namespace Toolkit.Foundation;

public class Validation(IValidatorCollection validators) :
    IValidation
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly ValidationErrorCollection errors = [];

    public IReadOnlyDictionary<string, string> Errors => 
        errors.AsReadOnly();

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
            Validate(name);
        }
    }

    public virtual void OnPropertyChanged(string propertyName,
        object? before, object? after)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    public bool Validate(string name)
    {
        if (Errors.ContainsKey(name))
        {
            errors.Remove(name);
        }

        if (Validators.TryGet(name, out Validator? validator))
        {
            if (validator is not null)
            {
                if (!validator.TryValidate(out string? message))
                {
                    errors[name] = message ?? "";
                }
            }
        }

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);

        return !HasErrors;
    }

    public bool Validate()
    {
        errors.Clear();
        foreach (Validator? validator in Validators)
        {
            if (validator.PropertyName is string name)
            {
                if (!validator.TryValidate(out string? message))
                {
                    errors[name] = message ?? "";
                }
            }
        }

        OnPropertyChanged(nameof(Errors), null, null);
        OnPropertyChanged(nameof(HasErrors), null, null);

        return !HasErrors;
    }

    private string GetPropertyName<T>(Expression<Func<T>> predicate)
    {
        Expression? body = predicate.Body;
        MemberExpression? memberExpression = body as MemberExpression ??
            (MemberExpression)((UnaryExpression)body).Operand;

        return memberExpression.Member.Name;
    }
}
