using System.Threading;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Features.BalanceBar
{
    public class BalanceBarViewController : ControllerBase
    {
        private readonly IAssetProvider _assetProvider;
        private readonly ISkuRegistrationService _skuRegistrationService;

        public BalanceBarViewController(IControllerFactory controllerFactory,
                                        IAssetProvider assetProvider,
                                        ISkuRegistrationService skuRegistrationService)
            : base(controllerFactory)
        {
            _assetProvider = assetProvider;
            _skuRegistrationService = skuRegistrationService;
        }
        protected override async UniTask AsyncFlow(object context, CancellationToken flowToken)
        {
            var balanceBarViewContext = (BalanceBarViewContext)context;
            
            using var prefabHolder =
                await _assetProvider.LoadAsync<GameObject>(balanceBarViewContext.BalanceBarPrefabAddress, flowToken);
            var instance = Object.Instantiate(prefabHolder.Asset, balanceBarViewContext.Parent);
            using var _ = instance.ToDisposable();
            var view = instance.GetComponent<BalanceBarView>();

            var itemContext = new BalanceBarItemContext();
            itemContext.Parent = view.ItemsContainer;
            itemContext.Prefab = view.ItemPrefab;

            foreach (var skuHandler in _skuRegistrationService.SkuHandlers)
            {
                var ctx = itemContext with { SkuHandler = skuHandler };
                StartAndWait<BalanceBarItemController>(ctx, flowToken).Forget();
            }

            await UniTask.WaitUntilCanceled(flowToken);
        }
    }
}