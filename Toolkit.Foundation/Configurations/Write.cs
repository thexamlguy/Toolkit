using Mediator;

namespace Toolkit.Foundation;

public abstract record Write<TConfiguration>(string Section, Action<TConfiguration> UpdateDelegate) : IRequest  where TConfiguration : class;
