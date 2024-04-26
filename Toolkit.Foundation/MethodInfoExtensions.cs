using System.Reflection;

namespace Toolkit.Foundation;

public static class MethodInfoExtensions
{
    public static async Task<TResult> InvokeAsync<TResult>(this MethodInfo methodInfo,
        object? obj)
    {
        dynamic result = await (dynamic?)methodInfo.Invoke(obj, null);
        return (TResult)result;
    }

    public static async Task InvokeAsync(this MethodInfo methodInfo,
        object? obj,
        params object[] parameters)
    {
        await (dynamic?)methodInfo.Invoke(obj, parameters);
    }
}