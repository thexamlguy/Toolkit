using Mediator;

namespace Toolkit.Framework.Foundation;

public abstract record Write<TConfiguration>(string Section, Action<TConfiguration> UpdateDelegate) : IRequest where TConfiguration : class;