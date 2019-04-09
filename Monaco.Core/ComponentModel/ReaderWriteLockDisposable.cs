using System;
using System.Threading;

namespace Monaco.Core.ComponentModel
{
    public class ReaderWriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _locker;
        private readonly ReaderWriterLockType _lockerType;

        public ReaderWriteLockDisposable(ReaderWriterLockSlim locker, ReaderWriterLockType lockerType)
        {
            this._locker = locker;
            this._lockerType = lockerType;

            switch (lockerType)
            {
                case ReaderWriterLockType.Write:
                    locker.EnterWriteLock();
                    break;
                case ReaderWriterLockType.Read:
                    locker.EnterReadLock();
                    break;
                case ReaderWriterLockType.UpgradeableRead:
                    locker.EnterUpgradeableReadLock();
                    break;
                case ReaderWriterLockType.None:
                    break;
                default:
                    break;
            }
        }

        void IDisposable.Dispose()
        {
            switch (_lockerType)
            {
                case ReaderWriterLockType.Read:
                    _locker.ExitReadLock();
                    break;
                case ReaderWriterLockType.Write:
                    _locker.ExitWriteLock();
                    break;
                case ReaderWriterLockType.UpgradeableRead:
                    _locker.ExitUpgradeableReadLock();
                    break;
            }
        }
    }

    /// <summary>
    /// Reader/Write locker type
    /// </summary>
    public enum ReaderWriterLockType
    {
        None,
        Read,
        Write,
        UpgradeableRead
    }
}
