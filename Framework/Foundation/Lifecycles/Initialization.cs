using System.Diagnostics;

namespace Toolkit.Framework.Foundation;

public class Initialization : IInitialization
{
    private readonly Func<IEnumerable<IInitializable?>> factory;

    public Initialization(Func<IEnumerable<IInitializable?>> factory)
    {
        this.factory = factory;
    }

    public async Task InitializeAsync()
    {
        foreach (IInitializable? initializer in factory())
        {
            if (initializer is not null)
            {
                Trace.WriteLine(initializer.GetType());
                await initializer.InitializeAsync();
                Trace.WriteLine("Done");
            }
        }
    }
}