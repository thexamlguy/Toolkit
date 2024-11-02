using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public interface IComponentHost :
    IHost
{
    TConfiguration? GetConfiguration<TConfiguration>()
        where TConfiguration : 
        class;
}