namespace Toolkit.Foundation
{
    public interface INavigationRouter : IInitializer
    {
        void Navigate(Navigate args);

        void Register(string name, object route);
    }
}