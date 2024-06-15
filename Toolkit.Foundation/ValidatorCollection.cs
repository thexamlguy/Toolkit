using System.Diagnostics.CodeAnalysis;
using System.Collections;

namespace Toolkit.Foundation;

public class ValidatorCollection :
    IValidatorCollection
{
    private readonly Dictionary<string, Validator> binders = [];

    public int Count => binders.Count;

    public void Add(string key, Validator binder) => 
        binders.Add(key, binder);

    public IEnumerator<Validator> GetEnumerator() => 
        binders.Select(x => x.Value).GetEnumerator();

    public bool TryGet(string key, [MaybeNull] out Validator? value) => 
        binders.TryGetValue(key, out value);

    IEnumerator IEnumerable.GetEnumerator() => 
        binders.Select(x => x.Value).GetEnumerator();
}
