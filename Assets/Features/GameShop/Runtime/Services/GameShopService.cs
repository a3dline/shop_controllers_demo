using System.Threading;
using AUniTaskSemaphore;
using Cysharp.Threading.Tasks;

namespace Features.GameShop
{
    public interface IGameShopService
    {
        public UniTask<GameShopData> GetShopDataAsync(CancellationToken token);
        public void UpdateData(GameShopData data);
    }

    internal class GameShopService : IGameShopService
    {
        private GameShopData _data;
        private bool _isDataInitialized;
        private UniTaskCompletionSource<GameShopData> _dataGetterTask;
        private readonly UniTaskSemaphore _getterSemaphore = new(1);

        public async UniTask<GameShopData> GetShopDataAsync(CancellationToken token)
        {
            using var _ = await _getterSemaphore.Acquire(token);
            
            if (!_isDataInitialized)
            {
                _dataGetterTask = new UniTaskCompletionSource<GameShopData>();
                await _dataGetterTask.Task.AttachExternalCancellation(token);
            }
            
            return _data;
        }
        public void UpdateData(GameShopData data)
        {
            _data = data;
            _isDataInitialized = true;
            _dataGetterTask?.TrySetResult(data);
        }
    }
}