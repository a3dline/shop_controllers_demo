using System.Threading;
using AControllersTree;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.BalanceBar
{
    public class BalanceBarItemController : ControllerBase
    {
        private readonly IPlayerDataRepository _playerData;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public BalanceBarItemController(IControllerFactory controllerFactory,
                                        ISkuRegistrationService skuRegistrationService,
                                        IPlayerDataRepository playerData) : base(controllerFactory)
        {
            _skuRegistrationService = skuRegistrationService;
            _playerData = playerData;
        }

        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var itemContext = (BalanceBarItemContext)context;
            var prefab = itemContext.Prefab;
            var parent = itemContext.Parent;
            var skuHandler = itemContext.SkuHandler;

            var instance = Object.Instantiate(prefab, parent);
            using var _ = instance.ToDisposable();

            var view = instance.GetComponent<BalanceBarItemView>();
            var displayName = _skuRegistrationService.GetSkuDisplayName(skuHandler.SkuId);
            view.SetLabel(displayName);
            view.PlusButtonWasClicked += () => AddValueToRepository(skuHandler, flowToken);

            await foreach (var value in skuHandler.BalanceString.WithCancellation(flowToken))
            {
                view.SetBalance(value);
            }
        }

        private void AddValueToRepository(ISkuHandler skuHandler, CancellationToken token)
        {
            var newBalance = skuHandler.Add(skuHandler.Balance, skuHandler.DefaultBalance);
            _playerData.UpdateSku(skuHandler.SkuId, newBalance, token);
        }
    }
}