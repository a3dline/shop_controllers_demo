using System.Globalization;
using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Linq;
using UnityEngine;

namespace Features.LocationSku
{
    internal class LocationSkuHandlerController : ControllerBase
    {
        private readonly IPlayerDataRepositoryWrapper _playerDataRepository;

        private readonly ISkuHandlerInternal _skuHandler;

        public LocationSkuHandlerController(IControllerFactory controllerFactory,
                                            ISkuHandlerInternal skuHandler,
                                            IPlayerDataRepositoryWrapper playerDataRepository) : base(controllerFactory)
        {
            _skuHandler = skuHandler;
            _playerDataRepository = playerDataRepository;
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var skuId = (string)context;
            var location = await _playerDataRepository.GetSkuData(skuId);
            if (location != null)
            {
                location = "default";
            }

            _skuHandler.UpdateBalance(location);

            await foreach (var _ in UniTaskAsyncEnumerable.EveryUpdate().WithCancellation(flowToken))
            {
                if (!_skuHandler.IsDirty)
                {
                    continue;
                }

                var newBalance = _skuHandler.TakeNewBalanceAndClear();

                Debug.Log("Updated current location: " + newBalance);

                await _playerDataRepository.UpdateSku(skuId, newBalance.ToString(CultureInfo.InvariantCulture));
                _skuHandler.UpdateBalance(newBalance);
            }
        }
    }
}