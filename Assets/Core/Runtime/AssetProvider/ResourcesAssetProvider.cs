using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core.AssetProvider
{
    internal class ResourcesAssetProvider : IAssetProvider
    {
        public async UniTask<AssetHolder<T>> LoadAsync<T>(string address, CancellationToken token)
            where T : Object
        {
            var asset = await Resources.LoadAsync<T>(address).ToUniTask(cancellationToken: token);
            if (asset == null)
            {
                throw new Exception($"Failed to load asset at address: {address}");
            }

            return new AssetHolder<T>(asset as T, () => Resources.UnloadAsset(asset));
        }
    }
}