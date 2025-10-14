using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Core.Disposables
{
    public sealed class CompositeDisposable : DisposableBase
    {
        private readonly List<IDisposable> _disposables;
        private readonly IDisposable _listToken;

        public CompositeDisposable()
        {
            _listToken = ListPool<IDisposable>.Get(out _disposables);
        }

        public void Add(IDisposable disposable)
        {
            ThrowIfDisposed();
            _disposables.Add(disposable);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (var disposable in _disposables)
                {
                    disposable.Dispose();
                }

                _listToken.Dispose();
            }
        }
    }
}