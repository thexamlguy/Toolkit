using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public interface IComponentHost :
    IHost
{
    ComponentConfiguration? Configuration { get; }
}