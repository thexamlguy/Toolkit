using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Framework.Foundation;

public interface ITemplateFactory
{
    object? Create([MaybeNull] object? data);
}