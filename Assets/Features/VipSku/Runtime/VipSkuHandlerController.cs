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
            var repositoryProperty = await _playerDataRepository.GetSkuPropertyAsync(skuId, flowToken);

            UpdateBalanceAsyncFlow(repositoryProperty, flowToken).Forget();
            TransactionsAsyncFlow(skuId, flowToken).Forget();

            var now = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            if (repositoryProperty.Value is not null)
            {
                var value = repositoryProperty.Value.ToInt64(CultureInfo.InvariantCulture);
                await _playerDataRepository.UpdateSku(skuId, value < now ? now : value, flowToken);
            }
            else
            {
                await _playerDataRepository.UpdateSku(skuId, now, flowToken);
            }

            await UniTask.WaitUntilCanceled(flowToken);
        }
        
        private async UniTaskVoid UpdateBalanceAsyncFlow(IReadOnlyAsyncReactiveProperty<IConvertible> property,
                                                         CancellationToken flowToken)
        {
            await foreach (var value in property.WithCancellation(flowToken))
            {
                _skuHandler.UpdateBalance(value);
            }
        }

        private async UniTaskVoid TransactionsAsyncFlow(string skuId, CancellationToken flowToken)
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

                await _playerDataRepository.UpdateSku(skuId, newBalance.ToString(CultureInfo.InvariantCulture), flowToken);
                _skuHandler.UpdateBalance(newBalance);
            }
        }
    }
}