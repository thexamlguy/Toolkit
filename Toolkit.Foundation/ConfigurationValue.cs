namespace Toolkit.Foundation;

public class ConfigurationValue<TConfiguration, TValue>(Func<TConfiguration, Action<TValue>> changed)
    where TValue :
    class, new()
{
    private TValue? currentValue;

    public bool TryUpdate(TConfiguration configuration,
        out TValue value)
    {
        TValue newValue = new();
        changed(configuration).Invoke(newValue);

        if (!EqualityComparer<TValue>.Default.Equals(currentValue, newValue))
        {
            value = newValue;
            currentValue = newValue;
            return true;
        }

        value = currentValue!;
        return false;
    }
}