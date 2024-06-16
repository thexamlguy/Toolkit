using System.ComponentModel;
using System.Linq.Expressions;

namespace Toolkit.Foundation;

public interface IValidation : 
    INotifyPropertyChanged
{
    ValidationErrorCollection Errors { get; }

    bool HasErrors { get; }

    void Add<TProperty>(Expression<Func<TProperty>> property,
        ValidationRule[] rules,
        ValidationTrigger trigger = ValidationTrigger.Deferred);

    bool Validate();

    bool Validate(string name);
}