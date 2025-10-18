using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Core
{
    internal class AddressableAssetProvider : IAssetProvider
    {
        public async UniTask<AssetHolder<T>> LoadAsync<T>(string address, CancellationToken token)
            where T : Object
        {
            var handler = Addressables.LoadAssetAsync<T>(address);
            await handler.ToUniTask(cancellationToken: token);

            return new AssetHolder<T>(handler.Result, () => Addressables.Release(handler));
        }
    }
}