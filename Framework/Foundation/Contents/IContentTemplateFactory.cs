using System.Diagnostics.CodeAnalysis;

namespace Toolkit.Framework.Foundation;

public interface IContentTemplateFactory
{
    object? Create([MaybeNull] object? content);
}