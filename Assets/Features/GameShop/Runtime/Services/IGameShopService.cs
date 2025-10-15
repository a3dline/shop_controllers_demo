using System.Threading;
using Cysharp.Threading.Tasks;

namespace Features.GameShop
{
    public interface IGameShopService
    {
        public UniTask<GameShopData> GetShopDataAsync(CancellationToken token);
        public void UpdateData(GameShopData data);
        public UniTask PurchaseItemAsync(in BundleData itemId, CancellationToken token);
    }
}