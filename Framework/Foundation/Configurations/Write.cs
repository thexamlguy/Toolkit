using Mediator;

namespace Toolkit.Framework.Foundation;

public record Write<TConfiguration>(Action<TConfiguration> UpdateDelegate) : IRequest where TConfiguration : class;