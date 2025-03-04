﻿using System.ComponentModel;
using System.Linq.Expressions;

namespace Toolkit.Foundation;

public interface IValidation :
    INotifyPropertyChanged
{
    IReadOnlyIndexDictionary<string, string> Errors { get; }

    bool HasErrors { get; }

    void Add<TProperty>(Expression<Func<TProperty>> property,
        ValidationRule[] rules,
        ValidationTrigger trigger = ValidationTrigger.Deferred);

    void Clear();

    Task<bool> Validate<TProperty>(Expression<Func<TProperty>> property,
        ValidationRule[] rules);

    Task<bool> Validate(string name,
        ValidationRule[] rules);

    Task<bool> Validate();

    Task<bool> Validate(string name);
}