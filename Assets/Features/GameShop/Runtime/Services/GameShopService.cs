using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using AUniTaskSemaphore;
using Core;
using Core.BackedClient;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.GameShop
{
    internal class GameShopService : IGameShopService
    {
        private readonly IBackendClient _backendClient;
        private readonly UniTaskSemaphore _getterSemaphore = new(1);
        private readonly ISkuRegistrationService _skuRegistrationService;
        private GameShopData _data;
        private UniTaskCompletionSource<GameShopData> _dataGetterTask;
        private bool _isDataInitialized;
        private Dictionary<string, AsyncReactiveProperty<bool>> _purchasingBundlesProperty = new();

        public GameShopService(IBackendClient backendClient,
                               ISkuRegistrationService skuRegistrationService)
        {
            _backendClient = backendClient;
            _skuRegistrationService = skuRegistrationService;
        }

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

        public async UniTask<bool> PurchaseItemAsync(BundleData bundle, CancellationToken token)
        {
            if (!_purchasingBundlesProperty.TryGetValue(bundle.BundleId, out var property))
            {
                property = new AsyncReactiveProperty<bool>(false);
                _purchasingBundlesProperty[bundle.BundleId] = property;
            }
            property.Value = true;
            
            var requestDto = new BundleRequestDto
                             {
                                 BundleId = bundle.BundleId
                             };

            var response =
                await _backendClient.PostAsync<BundleRequestDto, BundleResponseDto>("game-shop/purchase-bundle",
                 requestDto,
                 token);

            if (response.Status == ResponseStatus.Failure)
            {
                Debug.LogError($"Purchase bundle {response.BundleId} failed: {response.Error}");
                return false;
            }

            foreach (var sku in bundle.PurchaseData.Concat(bundle.RewardData))
            {
                var handler = _skuRegistrationService.GetSkuHandler(sku.SkuId);
                if (handler is null)
                {
                    throw new Exception($"Failed to find sku handler for sku: {sku.SkuId}");
                }

                handler.AddTransaction(sku.Amount);
            }

            property.Value = false;
            return true;
        }

        public IReadOnlyAsyncReactivePropertyDisposable<bool> CanPurchaseItemProperty(BundleData bundle)
        {
            var handlers = bundle.PurchaseData
                                 .Select(sku => new ValueTuple<ISkuHandler, IConvertible>(_skuRegistrationService.GetSkuHandler(sku.SkuId), sku.Amount))
                                 .ToList();
            
            var purchasingProperty = _purchasingBundlesProperty.GetValueOrDefault(bundle.BundleId);
            
            return new CanPurchaseItemProperty(handlers, purchasingProperty);
        }
    }

    [Serializable]
    public record BundleRequestDto
    {
        public string BundleId;
    }

    public record BundleResponseDto
    {
        public string BundleId;
        public string Error;
        public ResponseStatus Status;
    }
}