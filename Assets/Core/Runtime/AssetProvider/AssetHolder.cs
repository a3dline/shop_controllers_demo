using System;
using Object = UnityEngine.Object;

namespace Core.AssetProvider
{
    public readonly struct AssetHolder<T> : IDisposable
        where T : Object
    {
        private readonly Action _releaseCallback;

        public AssetHolder(T asset, Action releaseCallback)
        {
            Asset = asset;
            _releaseCallback = releaseCallback;
        }

        public T Asset { get; }

        public void Dispose()
        {
            _releaseCallback?.Invoke();
        }
    }
}