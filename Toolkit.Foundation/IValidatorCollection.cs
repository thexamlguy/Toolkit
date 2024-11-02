using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation;

public interface IValidatorCollection :
    IReadOnlyCollection<Validator>
{
    void Add(string key, Validator binder);

    bool TryGet(string key, [MaybeNull] out Validator? value);
}