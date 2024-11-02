using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolkit.Foundation;

namespace Toolkit.WinUI;

public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddWinUI(this IServiceCollection services)
    {
        services.AddTransient<IDispatcherTimerFactory, DispatcherTimerFactory>();
        return services;
    }
}
