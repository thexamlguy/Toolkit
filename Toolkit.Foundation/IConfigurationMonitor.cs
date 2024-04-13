
using Microsoft.Extensions.Hosting;

namespace Toolkit.Foundation;

public interface IConfigurationMonitor<TConfiguration> : 
    IHostedService
    where TConfiguration :
    class;