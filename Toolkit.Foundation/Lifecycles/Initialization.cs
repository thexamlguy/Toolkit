namespace Toolkit.Foundation
{
    public class Initialization : IInitialization
    {
        private readonly Func<IEnumerable<IInitializer?>> factory;

        public Initialization(Func<IEnumerable<IInitializer?>> factory)
        {
            this.factory = factory;
        }

        public async Task InitializeAsync()
        {
            foreach (IInitializer? initializer in factory())
            {
                if (initializer is not null)
                {
                    await initializer.InitializeAsync();
                }
            }
        }
    }
}
