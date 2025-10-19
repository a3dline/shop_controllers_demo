using System.Threading;
using Core;
using Cysharp.Threading.Tasks;

namespace Features.GameShop
{
    public interface IGameShopService
    {
        public UniTask<GameShopData> GetShopDataAsync(CancellationToken token);
        public void UpdateData(GameShopData data);
        public UniTask<bool> PurchaseItemAsync(BundleData bundle, CancellationToken token);
        public IReadOnlyAsyncReactivePropertyDisposable<bool> CanPurchaseItemProperty(BundleData bundle);
    }
}