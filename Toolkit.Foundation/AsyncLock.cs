using System.Runtime.CompilerServices;

namespace Toolkit.Foundation;

public class ActivityLock(IActivityIndicator activityIndicator) : AsyncLock
{
    public override TaskAwaiter<AsyncLock> GetAwaiter()
    {
        activityIndicator.IsActive = true;
        return base.GetAwaiter();
    }

    public override void Dispose()
    {
        activityIndicator.IsActive = false;
        base.Dispose();
    }
}

public class AsyncLock(int initial = 1,
    int maximum = 1) :
    IDisposable
{
    private readonly SemaphoreSlim semaphore = new(initial, maximum);

    public virtual void Dispose()
    {
        semaphore.Release();
    }

    public virtual TaskAwaiter<AsyncLock> GetAwaiter() => LockAsync().GetAwaiter();

    private async Task<AsyncLock> LockAsync()
    {
        await semaphore.WaitAsync();
        return this;
    }
}