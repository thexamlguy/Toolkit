using Mediator;

namespace Toolkit.Foundation
{
    public record NavigationRoute(string Name, object Route) : IRequest;
}