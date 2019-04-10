using System;

namespace Monaco.Core.Caching
{
    /// <summary>
    /// Lock manager interface
    /// </summary>
    public interface ILockManager
    {
        IDisposable AcquireLock(string resource, TimeSpan expirationTime);
    }
}
