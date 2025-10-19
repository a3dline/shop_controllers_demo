using System;
using System.Globalization;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Features.VipSku
{
    internal class VipSkuHandlerController : ControllerBase
    {
        private readonly IPlayerDataRepositoryWrapper _playerDataRepository;
        private readonly ISkuHandlerInternal _skuHandler;

        public VipSkuHandlerController(IControllerFactory controllerFactory,
                                       ISkuHandlerInternal skuHandler,
                                       IPlayerDataRepositoryWrapper playerDataRepository)
            : base(controllerFactory)
        {
            _skuHandler = skuHandler;
            _playerDataRepository = playerDataRepository;
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var skuId = (string)context;
            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var cachedValueString = await _playerDataRepository.GetSkuData(skuId);
            if (long.TryParse(cachedValueString, out var cachedValue))
            {
                _skuHandler.UpdateBalance(cachedValue < now ? now : cachedValue);
            }
            else
            {
                _skuHandler.UpdateBalance(now);
            }

            TransactionsFlowAsync(skuId, flowToken).Forget();
            await UniTask.WaitUntilCanceled(flowToken);
        }

        private async UniTaskVoid TransactionsFlowAsync(string skuId, CancellationToken flowToken)
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(flowToken))
            {
                if (!_skuHandler.IsDirty)
                {
                    continue;
                }

                var newBalance = _skuHandler.TakeNewBalanceAndClear();

                Debug.Log("Updated VIP time: " + DateTimeOffset
                                                 .FromUnixTimeSeconds(newBalance.ToInt64(CultureInfo.InvariantCulture))
                                                 .DateTime);

                await _playerDataRepository.UpdateSku(skuId, newBalance.ToString(CultureInfo.InvariantCulture));
                _skuHandler.UpdateBalance(newBalance);
            }
        }
    }
}