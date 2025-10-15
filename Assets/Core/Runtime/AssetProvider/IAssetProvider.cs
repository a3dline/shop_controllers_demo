using System.Threading;
using Cysharp.Threading.Tasks;
using Object = UnityEngine.Object;

namespace Core.AssetProvider
{
    public interface IAssetProvider
    {
        UniTask<AssetHolder<T>> LoadAsync<T>(string address, CancellationToken token) where T : Object;
        
        public enum Type
        {
            Addressable,
            Resources
        }
    }
}