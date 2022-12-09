using Mediator;
using System.Diagnostics;

namespace Toolkit.Foundation;

public class InitializeHandler : IRequestHandler<Initialize>
{
    private readonly IEnumerable<IInitializable?> initializers;

    public InitializeHandler(IEnumerable<IInitializable?> initializers)
    {
        this.initializers = initializers;
    }

    public async ValueTask<Unit> Handle(Initialize request, CancellationToken cancellationToken)
    {
        foreach (IInitializable? initializer in initializers)
        {
            if (initializer is not null)
            {
                Trace.WriteLine(initializer.GetType());
                await initializer.InitializeAsync();
                Trace.WriteLine("Done");
            }
        }

        return default;
    }
}
