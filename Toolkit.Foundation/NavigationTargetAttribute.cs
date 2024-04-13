namespace HyperX.UI.Windows;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class NavigationTargetAttribute(string name) : 
    Attribute
{
    public string Name => name;
}
