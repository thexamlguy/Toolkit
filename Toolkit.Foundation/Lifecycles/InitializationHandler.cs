using Mediator;

namespace Toolkit.Foundation
{
    public class InitializationHandler : IRequestHandler<Initialization>
    {
        private readonly IEnumerable<IInitializer?> initializers;

        public InitializationHandler(IEnumerable<IInitializer?> initializers)
        {
            this.initializers = initializers;
        }

        public async ValueTask<Unit> Handle(Initialization request, CancellationToken cancellationToken)
        {
            foreach (IInitializer? initializer in initializers)
            {
                if (initializer is not null)
                {
                    await initializer.InitializeAsync();
                }
            }

            return default;
        }
    }
}
