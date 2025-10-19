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
        private const string SkuId = "health";
        private readonly IPlayerDataRepositoryWrapper _playerDataRepository;

        private readonly ISkuHandlerInternal _skuHandler;

        public VipSkuHandlerController(IControllerFactory controllerFactory,
                                       ISkuHandlerInternal skuHandler,
                                       IPlayerDataRepositoryWrapper playerDataRepository) : base(controllerFactory)
        {
            _skuHandler = skuHandler;
            _playerDataRepository = playerDataRepository;
        }

        protected override UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            _skuHandler.UpdateBalance(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            TransactionsFlowAsync(flowToken).Forget();
            UpdateBalanceFlowAsync(flowToken).Forget();
            return UniTask.WaitUntilCanceled(flowToken);
        }

        private async UniTaskVoid UpdateBalanceFlowAsync(CancellationToken flowToken)
        {
            await foreach (var _ in UniTaskAsyncEnumerable.Interval(TimeSpan.FromSeconds(1))
                                                          .WithCancellation(flowToken))
            {
                _skuHandler.UpdateBalance();
            }
        }

        private async UniTaskVoid TransactionsFlowAsync(CancellationToken flowToken)
        {
            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(flowToken))
            {
                if (!_skuHandler.IsDirty)
                {
                    continue;
                }

                var newBalance = _skuHandler.TakeNewBalanceAndClear();

                Debug.Log("Updated VIP time: " + DateTimeOffset.FromUnixTimeSeconds(newBalance.ToInt64(CultureInfo.InvariantCulture)).DateTime);

                await _playerDataRepository.UpdateSku(SkuId, newBalance.ToString(CultureInfo.InvariantCulture));
                _skuHandler.UpdateBalance(newBalance);
            }
        }
    }
}