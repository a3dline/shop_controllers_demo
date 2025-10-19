using System.Globalization;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Features.HealthSku
{
    internal class HealthSkuHandlerController : ControllerBase
    {
        private readonly IPlayerDataRepositoryWrapper _playerDataRepository;
        private readonly ISkuHandlerInternal _skuHandler;

        public HealthSkuHandlerController(IControllerFactory controllerFactory,
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
            var resultString = await _playerDataRepository.GetSkuData(skuId);
            if (!int.TryParse(resultString, out var balance))
            {
                balance = 100;
            }

            _skuHandler.UpdateBalance(balance);

            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(flowToken))
            {
                if (!_skuHandler.IsDirty)
                {
                    continue;
                }

                var newBalance = _skuHandler.TakeNewBalanceAndClear();

                Debug.Log("Updated health balance: " + newBalance);

                await _playerDataRepository.UpdateSku(skuId, newBalance.ToString(CultureInfo.InvariantCulture));
                _skuHandler.UpdateBalance(newBalance);
            }
        }
    }
}