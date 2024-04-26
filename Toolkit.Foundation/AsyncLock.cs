using System.Runtime.CompilerServices;

namespace Toolkit.Foundation;

public class AsyncLock(int initial = 1,
    int maximum = 1) :
    IDisposable
{
    private readonly SemaphoreSlim semaphore = new(initial, maximum);

    public void Dispose()
    {
        semaphore.Release();
    }

    public TaskAwaiter<AsyncLock> GetAwaiter() => LockAsync().GetAwaiter();

    private async Task<AsyncLock> LockAsync()
    {
        await semaphore.WaitAsync();
        return this;
    }
}