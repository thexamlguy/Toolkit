using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Foundation
{
    public interface ITemplateFactory
    {
        object? Create([MaybeNull] object? data);
    }
}
