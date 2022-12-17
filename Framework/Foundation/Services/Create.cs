namespace Toolkit.Framework.Foundation;

public record Create(Type Type, params object?[] Parameters) : IRequest<object?>;
