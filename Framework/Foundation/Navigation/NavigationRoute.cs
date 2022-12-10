using Mediator;

namespace Toolkit.Framework.Foundation;

public record NavigationRoute(string Name, object Route) : IRequest;