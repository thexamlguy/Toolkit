namespace Toolkit.Foundation;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class NavigationContextAttribute : Attribute
{
    public NavigationContextAttribute(string name)
    {
        Name = name;
    }

    public string Name { get; }
}