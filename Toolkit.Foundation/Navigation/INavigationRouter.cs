namespace Toolkit.Foundation
{
    public interface INavigationRouter
    {
        void Navigate(Navigate args);

        void Register(string name, object route);
    }
}