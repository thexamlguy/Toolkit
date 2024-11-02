namespace Toolkit.Foundation;

public static class ConfigurationLock
{
    private static readonly ReaderWriterLockSlim readerWriterLock = new();

    public static IDisposable EnterRead()
    {
        readerWriterLock.EnterReadLock();
        return new ConfigurationReaderLockDisposer(readerWriterLock);
    }

    public static IDisposable EnterWrite()
    {
        readerWriterLock.EnterWriteLock();
        return new ConfigurationWriterLockDisposer(readerWriterLock);
    }

    private class ConfigurationWriterLockDisposer(ReaderWriterLockSlim lockSlim) : 
        IDisposable
    {
        public void Dispose()
        {
            lockSlim.ExitWriteLock();
        }
    }

    private class ConfigurationReaderLockDisposer(ReaderWriterLockSlim lockSlim) :
        IDisposable
    {
        public void Dispose()
        {
            lockSlim.ExitReadLock();
        }
    }
}
