using System;
using UnityEngine;

namespace Core.Disposables
{
    public abstract class DisposableBase : IDisposable
    {
        public bool IsDisposed { get; private set; }

        public void Dispose()
        {
            if (IsDisposed)
            {
                Debug.LogError($"Object has been disposed twice {GetType().Name}");
                Dispose(false);
                return;
            }

            Dispose(true);
            IsDisposed = true;
        }

        public void ThrowIfDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected abstract void Dispose(bool disposing);
    }
}